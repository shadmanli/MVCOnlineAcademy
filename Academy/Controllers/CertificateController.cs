using Academy.Data;
using Academy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Academy.Controllers
{
    public class CertificateController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public CertificateController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET /Certificate/Verify?code=CERT-XXXX
        [HttpGet]
        public async Task<IActionResult> Verify(string code)
        {
            if (string.IsNullOrEmpty(code))
                return View("Verify", (object)null);

            var cert = await _context.Certificates
                .Include(c => c.AppUser)
                .Include(c => c.Course)
                    .ThenInclude(c => c.Instructor)
                .FirstOrDefaultAsync(c => c.CertificateNumber == code);

            return View("Verify", cert);
        }

        // GET /Certificate/My
        [Authorize(Roles = "User")]
        public async Task<IActionResult> My()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var certs = await _context.Certificates
                .Include(c => c.Course)
                    .ThenInclude(c => c.Instructor)
                .Where(c => c.AppUserId == user.Id && !c.IsRevoked)
                .OrderByDescending(c => c.IssuedAt)
                .ToListAsync();

            return View(certs);
        }

        // POST /Certificate/Claim — tələbə kursunu bitirib sertifikat istəyir
        [Authorize(Roles = "User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Claim(int courseId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            // Artıq sertifikat varsa
            var existing = await _context.Certificates
                .FirstOrDefaultAsync(c => c.AppUserId == user.Id && c.CourseId == courseId && !c.IsRevoked);
            if (existing != null)
                return Json(new { success = false, message = "Bu kurs üçün artıq sertifikatınız var.", certNumber = existing.CertificateNumber });

            // Ödənilib mi?
            bool paid = await (
                from oi in _context.OrderItems
                join o in _context.Orders on oi.OrderId equals o.Id
                where oi.CourseId == courseId
                      && o.AppUserId == user.Id
                      && o.Status == OrderStatus.Paid
                select oi.Id
            ).AnyAsync();
            if (!paid)
                return Json(new { success = false, message = "Bu kursu satın almamışsınız." });

            // Bütün videoları izləyib mi?
            var totalVideos = await _context.Videos.CountAsync(v => v.CourseId == courseId);
            var watchedCount = await _context.VideoProgresses
                .CountAsync(vp => vp.AppUserId == user.Id && vp.CourseId == courseId && vp.IsWatched);

            if (totalVideos > 0 && watchedCount < totalVideos)
                return Json(new { success = false, message = $"Bütün videoları izləməlisiniz. ({watchedCount}/{totalVideos})" });

            // Sertifikat yarat
            var certNumber = GenerateCertNumber();
            var cert = new Certificate
            {
                CertificateNumber = certNumber,
                AppUserId = user.Id,
                CourseId = courseId,
                IssuedAt = DateTime.Now
            };
            _context.Certificates.Add(cert);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Sertifikatınız uğurla verildi!", certNumber });
        }

        // GET /Certificate/Check/{courseId} — kursda sertifikat hüququ varmı?
        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> Check(int courseId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var totalVideos  = await _context.Videos.CountAsync(v => v.CourseId == courseId);
            var watchedCount = await _context.VideoProgresses
                .CountAsync(vp => vp.AppUserId == user.Id && vp.CourseId == courseId && vp.IsWatched);

            bool paid = await (
                from oi in _context.OrderItems
                join o in _context.Orders on oi.OrderId equals o.Id
                where oi.CourseId == courseId
                      && o.AppUserId == user.Id
                      && o.Status == OrderStatus.Paid
                select oi.Id
            ).AnyAsync();

            var existing = await _context.Certificates
                .FirstOrDefaultAsync(c => c.AppUserId == user.Id && c.CourseId == courseId && !c.IsRevoked);

            int pct = totalVideos > 0 ? (int)Math.Round((double)watchedCount / totalVideos * 100) : (totalVideos == 0 ? 100 : 0);
            bool eligible = paid && pct == 100 && existing == null;

            return Json(new
            {
                eligible,
                hasCert      = existing != null,
                certNumber   = existing?.CertificateNumber,
                paid,
                pct,
                watchedCount,
                totalVideos,
                missing      = !eligible && existing == null
                    ? (!paid ? "Kurs satın alınmayıb." : $"Videolar: {watchedCount}/{totalVideos}")
                    : ""
            });
        }

        // GET /Certificate/View/{certNumber}
        [HttpGet]
        [Route("Certificate/View/{certNumber}")]
        public async Task<IActionResult> View(string certNumber)
        {
            var cert = await _context.Certificates
                .Include(c => c.AppUser)
                .Include(c => c.Course)
                    .ThenInclude(c => c.Instructor)
                .FirstOrDefaultAsync(c => c.CertificateNumber == certNumber);

            if (cert == null) return NotFound();
            return View("CertificateView", cert);
        }

        private static string GenerateCertNumber()
        {
            var ts   = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString("X");
            var rand = Guid.NewGuid().ToString("N")[..6].ToUpper();
            return $"CERT-{ts}-{rand}";
        }
    }
}
