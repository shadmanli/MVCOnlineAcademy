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
                    // Raw SQL ilə birbaşa yoxla — EF mapping problemlərini tamamilə keç
                    var userId = user.Id;
                    var sql = $@"SELECT COUNT(1) FROM Orders o 
                                 INNER JOIN OrderItems oi ON o.Id = oi.OrderId 
                                 WHERE o.AppUserId = '{userId.Replace("'", "''")}' 
                                 AND oi.CourseId = {id}";
                    
                    var conn = _context.Database.GetDbConnection();
                    await conn.OpenAsync();
                    using var cmd = conn.CreateCommand();
                    cmd.CommandText = sql;
                    var result = await cmd.ExecuteScalarAsync();
                    await conn.CloseAsync();
                    
                    isPurchased = result != null && Convert.ToInt32(result) > 0;

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
