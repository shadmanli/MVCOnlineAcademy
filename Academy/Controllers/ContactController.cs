using Academy.Data;
using Academy.Models;
using Academy.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Academy.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;

        public ContactController(AppDbContext context, IEmailService emailService, IConfiguration config)
        {
            _context = context;
            _emailService = emailService;
            _config = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] ContactMessageRequest model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value!.Errors.Any())
                    .ToDictionary(
                        k => k.Key,
                        v => v.Value!.Errors.Select(e => e.ErrorMessage).FirstOrDefault()
                    );
                return Json(new { success = false, errors });
            }

            // DB-ə saxla
            var message = new ContactMessage
            {
                Name = model.Name.Trim(),
                Email = model.Email.Trim(),
                Website = model.Website?.Trim(),
                Message = model.Message.Trim(),
                CreatedAt = DateTime.Now
            };
            _context.ContactMessages.Add(message);
            await _context.SaveChangesAsync();

            // Admin emailinə bildiriş göndər
            try
            {
                var adminEmail = _config["SmtpSettings:AdminEmail"] ?? _config["SmtpSettings:SenderEmail"];
                if (!string.IsNullOrEmpty(adminEmail))
                {
                    var emailBody = $@"
                    <div style='font-family:Arial,sans-serif;max-width:600px;margin:0 auto;'>
                        <div style='background:linear-gradient(135deg,#667eea,#764ba2);padding:30px;border-radius:10px 10px 0 0;'>
                            <h2 style='color:white;margin:0;'>📩 Yeni Əlaqə Mesajı</h2>
                        </div>
                        <div style='background:#f9f9f9;padding:30px;border-radius:0 0 10px 10px;border:1px solid #eee;'>
                            <table style='width:100%;border-collapse:collapse;'>
                                <tr><td style='padding:10px 0;color:#666;width:120px;'><strong>Ad:</strong></td><td style='padding:10px 0;color:#333;'>{message.Name}</td></tr>
                                <tr><td style='padding:10px 0;color:#666;'><strong>Email:</strong></td><td style='padding:10px 0;'><a href='mailto:{message.Email}' style='color:#667eea;'>{message.Email}</a></td></tr>
                                {(string.IsNullOrEmpty(message.Website) ? "" : $"<tr><td style='padding:10px 0;color:#666;'><strong>Website:</strong></td><td style='padding:10px 0;'><a href='{message.Website}' style='color:#667eea;'>{message.Website}</a></td></tr>")}
                                <tr><td style='padding:10px 0;color:#666;'><strong>Tarix:</strong></td><td style='padding:10px 0;color:#333;'>{message.CreatedAt:dd.MM.yyyy HH:mm}</td></tr>
                            </table>
                            <div style='margin-top:20px;padding:20px;background:white;border-radius:8px;border-left:4px solid #667eea;'>
                                <strong style='color:#666;'>Mesaj:</strong>
                                <p style='color:#333;margin-top:10px;line-height:1.6;'>{message.Message}</p>
                            </div>
                        </div>
                    </div>";

                    await _emailService.SendEmailAsync(adminEmail, $"Yeni Mesaj: {message.Name}", emailBody);
                }
            }
            catch { /* Email xətası formanı bloklamasın */ }

            return Json(new { success = true, message = "Mesajınız uğurla göndərildi!" });
        }
    }

    public class ContactMessageRequest
    {
        [Required(ErrorMessage = "Ad boş ola bilməz.")]
        [StringLength(100, ErrorMessage = "Ad maksimum 100 simvol ola bilər.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Email boş ola bilməz.")]
        [EmailAddress(ErrorMessage = "Email formatı düzgün deyil.")]
        public string Email { get; set; } = null!;

        public string? Website { get; set; }

        [Required(ErrorMessage = "Mesaj boş ola bilməz.")]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "Mesaj minimum 10, maksimum 2000 simvol olmalıdır.")]
        public string Message { get; set; } = null!;
    }
}
