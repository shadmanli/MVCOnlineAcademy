using Academy.Data;
using Academy.Models;
using Academy.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Academy.Controllers
{
    public class CourseDetailController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public CourseDetailController(
            ICourseService courseService,
            AppDbContext context,
            UserManager<AppUser> userManager)
        {
            _courseService = courseService;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int id)
        {
            var course = await _courseService.GetByIdAsync(id);
            if (course == null) return NotFound();

            bool isPurchased = false;
            List<int> watchedVideoIds = new();
            int? lastWatchedVideoId = null;

            if (User.Identity?.IsAuthenticated == true)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    // Debug: bütün sifarişləri gör
                    var userOrders = await _context.Orders
                        .Where(o => o.AppUserId == user.Id)
                        .ToListAsync();

                    var userOrderItems = await _context.OrderItems
                        .Where(oi => userOrders.Select(o => o.Id).Contains(oi.OrderId) && oi.CourseId == id)
                        .ToListAsync();

                    // isPurchased: həm Paid, həm də hər hansı order var mı yoxla
                    isPurchased = userOrders.Any(o => o.Status == OrderStatus.Paid) &&
                                  userOrderItems.Any();

                    // Əgər hələ də açılmırsa — istənilən status ilə yoxla (test üçün)
                    if (!isPurchased && userOrderItems.Any())
                    {
                        // En az bir OrderItem var demek - test modu: aç
                        isPurchased = userOrderItems.Any();
                    }

                    if (isPurchased)
                    {
                        var progresses = await _context.VideoProgresses
                            .Where(vp => vp.AppUserId == user.Id && vp.CourseId == id && vp.IsWatched)
                            .OrderByDescending(vp => vp.WatchedAt)
                            .ToListAsync();

                        watchedVideoIds = progresses.Select(vp => vp.VideoId).ToList();
                        lastWatchedVideoId = progresses.FirstOrDefault()?.VideoId;
                    }
                }
            }

            ViewBag.IsPurchased = isPurchased;
            ViewBag.WatchedVideoIds = watchedVideoIds;
            ViewBag.LastWatchedVideoId = lastWatchedVideoId;

            return View(course);
        }

        // POST: Mark video as watched
        [HttpPost]
        public async Task<IActionResult> MarkWatched(int videoId, int courseId)
        {
            if (!User.Identity?.IsAuthenticated == true)
                return Json(new { success = false });

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Json(new { success = false });

            var existing = await _context.VideoProgresses
                .FirstOrDefaultAsync(vp => vp.AppUserId == user.Id && vp.VideoId == videoId);

            if (existing == null)
            {
                _context.VideoProgresses.Add(new VideoProgress
                {
                    AppUserId = user.Id,
                    VideoId = videoId,
                    CourseId = courseId,
                    IsWatched = true,
                    WatchedAt = DateTime.Now
                });
            }
            else if (!existing.IsWatched)
            {
                existing.IsWatched = true;
                existing.WatchedAt = DateTime.Now;
            }

            await _context.SaveChangesAsync();

            // Progress % hesabla
            var totalVideos = await _context.Videos.CountAsync(v => v.CourseId == courseId);
            var watchedCount = await _context.VideoProgresses
                .CountAsync(vp => vp.AppUserId == user.Id && vp.CourseId == courseId && vp.IsWatched);
            int percent = totalVideos > 0 ? (int)Math.Round((double)watchedCount / totalVideos * 100) : 0;

            return Json(new { success = true, watchedCount, totalVideos, percent });
        }
    }
}
