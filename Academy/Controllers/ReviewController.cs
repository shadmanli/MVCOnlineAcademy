using Academy.Data;
using Academy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Academy.Controllers
{
    public class ReviewController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ReviewController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // POST: /Review/Submit
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Submit(int courseId, int rating, string message)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Json(new { success = false, message = "Giriş etməlisiniz." });

            if (rating < 1 || rating > 5)
                return Json(new { success = false, message = "Reytinq 1-5 arasında olmalıdır." });

            if (string.IsNullOrWhiteSpace(message) || message.Length < 10)
                return Json(new { success = false, message = "Rəy minimum 10 simvol olmalıdır." });

            // Kursu satın alıb almadığını yoxla — Basket/Enrollment üzərindən
            // Enrollment cədvəlini yoxlayırıq (StudentId əvəzinə email ilə)
            var hasPurchased = await _context.Enrollments
                .Include(e => e.Student)
                .AnyAsync(e => e.CourseId == courseId && e.Student.Email == user.Email);

            if (!hasPurchased)
                return Json(new { success = false, message = "Yalnız kursu satın almış istifadəçilər rəy yaza bilər." });

            // Artıq rəy yazıb-yazmadığını yoxla
            var existingReview = await _context.CourseReviews
                .AnyAsync(r => r.AppUserId == user.Id && r.CourseId == courseId);

            if (existingReview)
                return Json(new { success = false, message = "Bu kursa artıq rəy yazmısınız." });

            var review = new CourseReview
            {
                AppUserId = user.Id,
                CourseId = courseId,
                Name = user.FullName ?? user.Email!,
                Photo = user.ProfilePicture,
                Rating = rating,
                Message = message.Trim(),
                Status = ReviewStatus.Pending,
                CreatedAt = DateTime.Now
            };

            _context.CourseReviews.Add(review);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Rəyiniz göndərildi. Təsdiq gözlənilir." });
        }

        // GET: /Review/CourseReviews?courseId=5
        [HttpGet]
        public async Task<IActionResult> CourseReviews(int courseId)
        {
            var reviews = await _context.CourseReviews
                .Where(r => r.CourseId == courseId && r.Status == ReviewStatus.Approved)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new
                {
                    r.Id,
                    r.Name,
                    r.Photo,
                    r.Rating,
                    r.Message,
                    Date = r.CreatedAt.ToString("dd.MM.yyyy")
                })
                .ToListAsync();

            return Json(reviews);
        }
    }
}
