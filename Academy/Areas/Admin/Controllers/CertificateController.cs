using Academy.Data;
using Academy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Academy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin,Muellim")]
    public class CertificateController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public CertificateController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string? search)
        {
            var query = _context.Certificates
                .Include(c => c.AppUser)
                .Include(c => c.Course)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim().ToLower();
                query = query.Where(c =>
                    c.CertificateNumber.ToLower().Contains(search) ||
                    (c.AppUser != null && c.AppUser.FullName.ToLower().Contains(search)) ||
                    (c.Course != null && c.Course.Title.ToLower().Contains(search)));
            }

            var certs = await query.OrderByDescending(c => c.IssuedAt).ToListAsync();
            ViewBag.Search = search;
            ViewBag.TotalCount = await _context.Certificates.CountAsync();
            ViewBag.ActiveCount = await _context.Certificates.CountAsync(c => !c.IsRevoked);
            ViewBag.RevokedCount = await _context.Certificates.CountAsync(c => c.IsRevoked);
            return View(certs);
        }

        // POST: Revoke
        [HttpPost]
        public async Task<IActionResult> Revoke(int id, string? reason)
        {
            var cert = await _context.Certificates.FindAsync(id);
            if (cert == null) return Json(new { success = false });
            cert.IsRevoked = true;
            cert.RevokedAt = DateTime.Now;
            cert.RevokeReason = reason;
            await _context.SaveChangesAsync();
            TempData["Success"] = "Sertifikat ləğv edildi.";
            return Json(new { success = true });
        }

        // POST: Restore
        [HttpPost]
        public async Task<IActionResult> Restore(int id)
        {
            var cert = await _context.Certificates.FindAsync(id);
            if (cert == null) return Json(new { success = false });
            cert.IsRevoked = false;
            cert.RevokedAt = null;
            cert.RevokeReason = null;
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        // POST: Manual issue
        [HttpGet]
        public async Task<IActionResult> Issue()
        {
            ViewBag.Users   = await _userManager.Users.Select(u => new SelectListItem(u.FullName ?? u.Email, u.Id)).ToListAsync();
            ViewBag.Courses = await _context.Courses.Select(c => new SelectListItem(c.Title, c.Id.ToString())).ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Issue(string userId, int courseId)
        {
            var existing = await _context.Certificates
                .FirstOrDefaultAsync(c => c.AppUserId == userId && c.CourseId == courseId && !c.IsRevoked);
            if (existing != null)
            {
                TempData["Error"] = "Bu istifadəçinin bu kurs üçün aktiv sertifikatı var.";
                return RedirectToAction(nameof(Index));
            }

            var cert = new Certificate
            {
                CertificateNumber = $"CERT-M-{DateTimeOffset.UtcNow.ToUnixTimeSeconds():X}-{Guid.NewGuid().ToString("N")[..5].ToUpper()}",
                AppUserId = userId,
                CourseId = courseId,
                IssuedAt = DateTime.Now,
                IsManual = true
            };
            _context.Certificates.Add(cert);
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Sertifikat uğurla verildi: {cert.CertificateNumber}";
            return RedirectToAction(nameof(Index));
        }
    }
}
