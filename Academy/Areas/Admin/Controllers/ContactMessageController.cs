using Academy.Data;
using Academy.Models;
using Academy.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Academy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class ContactMessageController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;

        public ContactMessageController(AppDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // GET: /Admin/ContactMessage
        public async Task<IActionResult> Index(string? search, string? filter)
        {
            var query = _context.ContactMessages.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();
                query = query.Where(m =>
                    m.Name.ToLower().Contains(search) ||
                    m.Email.ToLower().Contains(search));
            }

            if (filter == "unread")
                query = query.Where(m => !m.IsRead);
            else if (filter == "read")
                query = query.Where(m => m.IsRead);
            else if (filter == "replied")
                query = query.Where(m => m.IsReplied);

            var messages = await query.OrderByDescending(m => m.CreatedAt).ToListAsync();

            // Statistika
            var now = DateTime.Now;
            var weekStart = now.AddDays(-(int)now.DayOfWeek);
            ViewBag.TotalCount = await _context.ContactMessages.CountAsync();
            ViewBag.UnreadCount = await _context.ContactMessages.CountAsync(m => !m.IsRead);
            ViewBag.ThisWeekCount = await _context.ContactMessages.CountAsync(m => m.CreatedAt >= weekStart);
            ViewBag.RepliedCount = await _context.ContactMessages.CountAsync(m => m.IsReplied);
            ViewBag.Search = search;
            ViewBag.Filter = filter;

            return View(messages);
        }

        // POST: /Admin/ContactMessage/MarkRead
        [HttpPost]
        public async Task<IActionResult> MarkRead(int id)
        {
            var msg = await _context.ContactMessages.FindAsync(id);
            if (msg == null) return Json(new { success = false });

            msg.IsRead = true;
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        // POST: /Admin/ContactMessage/Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var msg = await _context.ContactMessages.FindAsync(id);
            if (msg == null) return Json(new { success = false, message = "Mesaj tapılmadı." });

            _context.ContactMessages.Remove(msg);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Mesaj silindi." });
        }

        // POST: /Admin/ContactMessage/Reply
        [HttpPost]
        public async Task<IActionResult> Reply(int id, string replyText)
        {
            var msg = await _context.ContactMessages.FindAsync(id);
            if (msg == null) return Json(new { success = false, message = "Mesaj tapılmadı." });

            if (string.IsNullOrWhiteSpace(replyText))
                return Json(new { success = false, message = "Cavab mətni boş ola bilməz." });

            try
            {
                var emailBody = $@"
                <div style='font-family:Arial,sans-serif;max-width:600px;margin:0 auto;'>
                    <div style='background:linear-gradient(135deg,#667eea,#764ba2);padding:30px;border-radius:10px 10px 0 0;'>
                        <h2 style='color:white;margin:0;'>📬 Mesajınıza Cavab</h2>
                    </div>
                    <div style='background:#f9f9f9;padding:30px;border-radius:0 0 10px 10px;border:1px solid #eee;'>
                        <p style='color:#333;'>Salam <strong>{msg.Name}</strong>,</p>
                        <p style='color:#666;'>Əvvəlcədən göndərdiyiniz mesaja cavab:</p>
                        <div style='background:white;padding:20px;border-radius:8px;border-left:4px solid #667eea;margin:20px 0;'>
                            <p style='color:#333;line-height:1.6;'>{replyText}</p>
                        </div>
                        <hr style='border:none;border-top:1px solid #eee;margin:20px 0;'>
                        <p style='color:#999;font-size:12px;'>Bu email Academy tərəfindən göndərilmişdir.</p>
                    </div>
                </div>";

                await _emailService.SendEmailAsync(msg.Email, "Mesajınıza Cavab - Academy", emailBody);

                msg.IsReplied = true;
                msg.IsRead = true;
                msg.RepliedAt = DateTime.Now;
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Cavab uğurla göndərildi!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Email göndərilə bilmədi: {ex.Message}" });
            }
        }

        // GET: /Admin/ContactMessage/GetMessage/5
        [HttpGet]
        public async Task<IActionResult> GetMessage(int id)
        {
            var msg = await _context.ContactMessages.FindAsync(id);
            if (msg == null) return Json(new { success = false });

            // Oxunmuş işarələ
            if (!msg.IsRead)
            {
                msg.IsRead = true;
                await _context.SaveChangesAsync();
            }

            return Json(new
            {
                success = true,
                id = msg.Id,
                name = msg.Name,
                email = msg.Email,
                website = msg.Website,
                message = msg.Message,
                createdAt = msg.CreatedAt.ToString("dd.MM.yyyy HH:mm"),
                isReplied = msg.IsReplied,
                repliedAt = msg.RepliedAt?.ToString("dd.MM.yyyy HH:mm")
            });
        }
    }
}
