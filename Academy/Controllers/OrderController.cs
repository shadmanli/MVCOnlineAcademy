using Academy.Data;
using Academy.Models;
using Academy.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;

namespace Academy.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IBasketService _basketService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;

        public OrderController(
            AppDbContext context,
            IBasketService basketService,
            UserManager<AppUser> userManager,
            IEmailService emailService,
            IConfiguration config)
        {
            _context = context;
            _basketService = basketService;
            _userManager = userManager;
            _emailService = emailService;
            _config = config;
            StripeConfiguration.ApiKey = config["Stripe:SecretKey"];
        }

        // GET: /Order/Checkout
        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            var user = await _userManager.GetUserAsync(User);
            var basketIds = _basketService.GetBasketCourseIds();

            if (!basketIds.Any())
                return RedirectToAction("Index", "Basket");

            var courses = await _context.Courses
                .Where(c => basketIds.Contains(c.Id))
                .ToListAsync();

            ViewBag.User = user;
            ViewBag.Courses = courses;
            ViewBag.Total = courses.Sum(c => c.Price);
            ViewBag.StripePublishableKey = _config["Stripe:PublishableKey"];

            return View();
        }

        // POST: /Order/CreatePaymentIntent
        [HttpPost]
        public async Task<IActionResult> CreatePaymentIntent([FromBody] CreatePaymentRequest req)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var basketIds = _basketService.GetBasketCourseIds();

                if (!basketIds.Any())
                    return Json(new { error = "Səbət boşdur." });

                // Qiymətləri həmişə backend-dən hesabla
                var courses = await _context.Courses
                    .Where(c => basketIds.Contains(c.Id))
                    .ToListAsync();

                var totalAmount = courses.Sum(c => c.Price);
                var amountInCents = (long)(totalAmount * 100);

                var options = new PaymentIntentCreateOptions
                {
                    Amount = amountInCents,
                    Currency = "usd",
                    AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                    {
                        Enabled = true
                    },
                    Metadata = new Dictionary<string, string>
                    {
                        { "userId", user!.Id },
                        { "userEmail", user.Email! },
                        { "courseIds", string.Join(",", basketIds) }
                    }
                };

                var service = new PaymentIntentService();
                var intent = await service.CreateAsync(options);

                return Json(new { clientSecret = intent.ClientSecret, amount = totalAmount });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        // POST: /Order/ConfirmOrder
        [HttpPost]
        public async Task<IActionResult> ConfirmOrder([FromBody] ConfirmOrderRequest req)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var basketIds = _basketService.GetBasketCourseIds();

                var courses = await _context.Courses
                    .Where(c => basketIds.Contains(c.Id))
                    .ToListAsync();

                if (!courses.Any())
                    return Json(new { success = false, message = "Kurs tapılmadı." });

                // Stripe PaymentIntent yoxla
                var service = new PaymentIntentService();
                var intent = await service.GetAsync(req.PaymentIntentId);

                if (intent.Status != "succeeded")
                    return Json(new { success = false, message = "Ödəniş təsdiqlənmədi." });

                // Sifariş yarat
                var order = new Models.Order
                {
                    AppUserId = user!.Id,
                    OrderNumber = "ORD-" + DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + new Random().Next(1000, 9999),
                    FullName = req.FullName,
                    Email = req.Email,
                    Phone = req.Phone,
                    TotalAmount = courses.Sum(c => c.Price),
                    PaymentMethod = "card",
                    StripePaymentIntentId = req.PaymentIntentId,
                    Status = OrderStatus.Paid,
                    CreatedAt = DateTime.Now,
                    OrderItems = courses.Select(c => new OrderItem
                    {
                        CourseId = c.Id,
                        CourseTitle = c.Title,
                        Price = c.Price
                    }).ToList()
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Səbəti təmizlə
                foreach (var id in basketIds.ToList())
                    _basketService.RemoveFromBasket(id);

                // Email göndər
                try
                {
                    var emailBody = BuildOrderConfirmationEmail(order, courses);
                    await _emailService.SendEmailAsync(req.Email, "Sifarişiniz Təsdiqləndi — Inzara Academy", emailBody);
                }
                catch { /* Email xətası sifarişi bloklamasın */ }

                return Json(new { success = true, orderNumber = order.OrderNumber, orderId = order.Id });
            }
            catch (Exception ex)
            {
                var innerMsg = ex.InnerException?.Message ?? ex.Message;
                return Json(new { success = false, message = innerMsg });
            }
        }

        // GET: /Order/Success/{id}
        [HttpGet]
        public async Task<IActionResult> Success(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Course)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return NotFound();

            var userId = _userManager.GetUserId(User);
            if (order.AppUserId != userId) return Forbid();

            return View(order);
        }

        // Stripe Webhook
        [HttpPost]
        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Webhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var webhookSecret = _config["Stripe:WebhookSecret"];

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    webhookSecret);

                if (stripeEvent.Type == "payment_intent.succeeded")
                {
                    var intent = stripeEvent.Data.Object as PaymentIntent;
                    var order = await _context.Orders
                        .FirstOrDefaultAsync(o => o.StripePaymentIntentId == intent!.Id);

                    if (order != null && order.Status != OrderStatus.Paid)
                    {
                        order.Status = OrderStatus.Paid;
                        await _context.SaveChangesAsync();
                    }
                }

                return Ok();
            }
            catch (StripeException)
            {
                return BadRequest();
            }
        }

        private string BuildOrderConfirmationEmail(Models.Order order, List<Models.Course> courses)
        {
            var items = string.Join("", courses.Select(c =>
                $"<tr><td style='padding:10px;border-bottom:1px solid #eee;'>{c.Title}</td><td style='padding:10px;border-bottom:1px solid #eee;text-align:right;'>{c.Price} AZN</td></tr>"));

            return $@"
            <div style='font-family:Inter,sans-serif;max-width:600px;margin:0 auto;background:#0d1117;color:#f1f5f9;border-radius:16px;overflow:hidden;'>
                <div style='background:linear-gradient(135deg,#6366f1,#0ea5e9);padding:32px;text-align:center;'>
                    <h1 style='color:white;margin:0;font-size:24px;'>✅ Sifarişiniz Təsdiqləndi!</h1>
                </div>
                <div style='padding:32px;'>
                    <p>Salam <strong>{order.FullName}</strong>,</p>
                    <p>Sifarişiniz uğurla tamamlandı. Sifariş nömrəniz: <strong style='color:#a78bfa;'>{order.OrderNumber}</strong></p>
                    <table style='width:100%;border-collapse:collapse;margin:20px 0;'>
                        <thead><tr style='background:rgba(99,102,241,0.2);'>
                            <th style='padding:10px;text-align:left;'>Kurs</th>
                            <th style='padding:10px;text-align:right;'>Qiymət</th>
                        </tr></thead>
                        <tbody>{items}</tbody>
                        <tfoot><tr>
                            <td style='padding:12px;font-weight:bold;'>Ümumi</td>
                            <td style='padding:12px;font-weight:bold;text-align:right;color:#a78bfa;'>{order.TotalAmount} AZN</td>
                        </tr></tfoot>
                    </table>
                    <p style='color:#94a3b8;font-size:13px;'>Kurslara daxil olmaq üçün saytımıza giriş edin.</p>
                </div>
            </div>";
        }
    }

    public class CreatePaymentRequest
    {
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
    }

    public class ConfirmOrderRequest
    {
        public string PaymentIntentId { get; set; } = "";
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
    }
}
