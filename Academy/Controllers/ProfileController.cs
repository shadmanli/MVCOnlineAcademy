using Academy.Models;
using Academy.ViewModels.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;

namespace Academy.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IWebHostEnvironment _env;

        public ProfileController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _env = env;
        }

        public async Task<IActionResult> Index([FromServices] Data.AppDbContext context)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            string[] names = user.FullName?.Split(' ');
            string firstName = names != null && names.Length > 0 ? names[0] : "";
            string lastName = names != null && names.Length > 1 ? string.Join(" ", names.Skip(1)) : "";

            var assessmentResults = await context.UserAssessmentResults
                .Include(r => r.Category)
                .Where(r => r.AppUserId == user.Id)
                .ToListAsync();

            ViewBag.User = user;
            ViewBag.FirstName = firstName;
            ViewBag.LastName = lastName;
            ViewBag.AssessmentResults = assessmentResults;

            return View();
        }

        [HttpPatch]
        [Route("api/user/profile")]
        public async Task<IActionResult> UpdateProfile([FromForm] ProfileUpdateDto model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            user.FullName = $"{model.FirstName} {model.LastName}".Trim();
            user.PhoneNumber = model.Phone;
            user.Bio = model.Bio;

            if (model.ProfilePicture != null)
            {
                if (model.ProfilePicture.Length > 2 * 1024 * 1024)
                    return BadRequest("Şəkil 2MB-dan böyük olmamalıdır.");

                string ex = Path.GetExtension(model.ProfilePicture.FileName);
                if (ex != ".png" && ex != ".jpg" && ex != ".jpeg")
                    return BadRequest("Yalnız PNG, JPG, JPEG formatları qəbul edilir.");

                string folder = Path.Combine(_env.WebRootPath, "uploads", "profile");
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid() + ex;
                string filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfilePicture.CopyToAsync(stream);
                }

                if (!string.IsNullOrEmpty(user.ProfilePicture))
                {
                    string oldPath = Path.Combine(folder, user.ProfilePicture);
                    if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                }

                user.ProfilePicture = fileName;
            }

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
                return Ok(new { message = "Profil uğurla yeniləndi.", profilePicture = user.ProfilePicture });

            return BadRequest(result.Errors);
        }

        [HttpPost]
        [Route("api/user/change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                return Ok(new { message = "Şifrə uğurla yeniləndi." });
            }

            return BadRequest(result.Errors);
        }

        [HttpPatch]
        [Route("api/user/notifications")]
        public async Task<IActionResult> UpdateNotifications([FromBody] NotificationUpdateDto model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            user.NotifyNewLesson = model.NotifyNewLesson;
            user.NotifyDiscounts = model.NotifyDiscounts;
            user.NotifyCertificate = model.NotifyCertificate;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded) return Ok();

            return BadRequest();
        }

        [HttpDelete]
        [Route("api/user/account")]
        public async Task<IActionResult> DeleteAccount([FromBody] DeleteAccountDto model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            if (user.Email != model.Email) return BadRequest("Email doğru deyil.");

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                await _signInManager.SignOutAsync();
                return Ok(new { message = "Hesabınız silindi." });
            }

            return BadRequest();
        }

        // GET: /api/user/quiz-results
        [HttpGet]
        [Route("api/user/quiz-results")]
        public async Task<IActionResult> GetQuizResults([FromServices] Data.AppDbContext context)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var results = await context.UserAssessmentResults
                .Include(r => r.Category)
                .Where(r => r.AppUserId == user.Id)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new {
                    courseId = r.CategoryId,
                    courseName = r.Category != null ? r.Category.Name : "Ümumi",
                    score = r.Score,
                    totalQuestions = r.TotalQuestions,
                    percentage = r.Percentage,
                    level = r.Level,
                    xp = r.XP,
                    date = r.CreatedAt.ToString("dd.MM.yyyy")
                })
                .ToListAsync();

            return Ok(results);
        }

        // GET: /api/user/enrollments
        [HttpGet]
        [Route("api/user/enrollments")]
        public async Task<IActionResult> GetEnrollments([FromServices] Data.AppDbContext context)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            // Ödənilmiş sifarişlərdən kursları çək
            var orderItems = await context.OrderItems
                .Include(oi => oi.Order)
                .Include(oi => oi.Course)
                .ThenInclude(c => c.Instructor)
                .Include(oi => oi.Course)
                .ThenInclude(c => c.Category)
                .Where(oi => oi.Order.AppUserId == user.Id && oi.Order.Status == OrderStatus.Paid)
                .Select(oi => new {
                    courseId = oi.CourseId,
                    title = oi.Course.Title,
                    instructor = oi.Course.Instructor != null ? oi.Course.Instructor.FullName : "Inzara Academy",
                    category = oi.Course.Category != null ? oi.Course.Category.Name : "",
                    thumbnail = oi.Course.ImageUrl,
                    progress = 0,
                    status = "active",
                    enrollDate = oi.Order.CreatedAt.ToString("dd.MM.yyyy")
                })
                .Distinct()
                .ToListAsync();

            return Ok(orderItems);
        }

        // GET: /api/user/activity
        [HttpGet]
        [Route("api/user/activity")]
        public async Task<IActionResult> GetActivity([FromServices] Data.AppDbContext context)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var activities = new List<object>();

            // Quiz nəticələri
            var quizResults = await context.UserAssessmentResults
                .Include(r => r.Category)
                .Where(r => r.AppUserId == user.Id)
                .OrderByDescending(r => r.CreatedAt)
                .Take(5)
                .ToListAsync();

            foreach (var q in quizResults)
            {
                activities.Add(new {
                    type = "quiz",
                    description = $"\"{q.Category?.Name}\" quizini tamamladınız — {q.Percentage}%",
                    timestamp = q.CreatedAt,
                    timeAgo = GetTimeAgo(q.CreatedAt),
                    icon = "fa-check-circle",
                    color = "#10b981"
                });
            }

            // Sifarişlər
            var orders = await context.Orders
                .Where(o => o.AppUserId == user.Id)
                .OrderByDescending(o => o.CreatedAt)
                .Take(3)
                .ToListAsync();

            foreach (var o in orders)
            {
                activities.Add(new {
                    type = "purchase",
                    description = $"Sifariş tamamlandı — {o.TotalAmount} AZN",
                    timestamp = o.CreatedAt,
                    timeAgo = GetTimeAgo(o.CreatedAt),
                    icon = "fa-shopping-bag",
                    color = "#6366f1"
                });
            }

            return Ok(activities.OrderByDescending(a => ((dynamic)a).timestamp).Take(10));
        }

        private string GetTimeAgo(DateTime dt)
        {
            var diff = DateTime.Now - dt;
            if (diff.TotalMinutes < 1) return "İndi";
            if (diff.TotalMinutes < 60) return $"{(int)diff.TotalMinutes} dəq əvvəl";
            if (diff.TotalHours < 24) return $"{(int)diff.TotalHours} saat əvvəl";
            if (diff.TotalDays < 2) return "Dünən";
            if (diff.TotalDays < 7) return $"{(int)diff.TotalDays} gün əvvəl";
            return dt.ToString("dd.MM.yyyy");
        }
    }
}

