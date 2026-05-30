using Academy.Data;
using Academy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Academy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class ReviewController : Controller
    {
        private readonly AppDbContext _context;

        public ReviewController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Admin/Review
        public async Task<IActionResult> Index(string? filter)
        {
            var query = _context.CourseReviews
                .Include(r => r.Course)
                .Include(r => r.AppUser)
                .AsQueryable();

            if (filter == "pending")
                query = query.Where(r => r.Status == ReviewStatus.Pending);
            else if (filter == "approved")
                query = query.Where(r => r.Status == ReviewStatus.Approved);
            else if (filter == "rejected")
                query = query.Where(r => r.Status == ReviewStatus.Rejected);

            var reviews = await query.OrderByDescending(r => r.CreatedAt).ToListAsync();

            ViewBag.Filter = filter;
            ViewBag.TotalCount = await _context.CourseReviews.CountAsync();
            ViewBag.PendingCount = await _context.CourseReviews.CountAsync(r => r.Status == ReviewStatus.Pending);
            ViewBag.ApprovedCount = await _context.CourseReviews.CountAsync(r => r.Status == ReviewStatus.Approved);
            ViewBag.RejectedCount = await _context.CourseReviews.CountAsync(r => r.Status == ReviewStatus.Rejected);

            return View(reviews);
        }

        // POST: /Admin/Review/Approve
        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var review = await _context.CourseReviews.FindAsync(id);
            if (review == null) return Json(new { success = false });

            review.Status = ReviewStatus.Approved;
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Rəy təsdiqləndi." });
        }

        // POST: /Admin/Review/Reject
        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            var review = await _context.CourseReviews.FindAsync(id);
            if (review == null) return Json(new { success = false });

            review.Status = ReviewStatus.Rejected;
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Rəy rədd edildi." });
        }

        // POST: /Admin/Review/Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var review = await _context.CourseReviews.FindAsync(id);
            if (review == null) return Json(new { success = false });

            _context.CourseReviews.Remove(review);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Rəy silindi." });
        }
    }
}
