using Academy.Data;
using Academy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Academy.Controllers
{
    [Route("api/live")]
    [Authorize]
    public class LiveApiController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public LiveApiController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET /api/live/next — istifadəçinin növbəti canlı dərsi
        [HttpGet("next")]
        public async Task<IActionResult> GetNext()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var purchasedIds = await _context.OrderItems
                .Where(oi => _context.Orders.Any(o =>
                    o.Id == oi.OrderId &&
                    o.AppUserId == user.Id &&
                    o.Status == OrderStatus.Paid))
                .Select(oi => oi.CourseId)
                .Distinct()
                .ToListAsync();

            if (!purchasedIds.Any())
                return Ok(new { });

            var lc = await _context.LiveClasses
                .Include(l => l.Course)
                .Include(l => l.Instructor)
                .Where(l => purchasedIds.Contains(l.CourseId) &&
                            l.Status != LiveSessionStatus.Ended &&
                            l.Status != LiveSessionStatus.Canceled &&
                            l.ScheduledDate >= DateTime.Now.AddMinutes(-l.DurationMinutes))
                .OrderBy(l => l.ScheduledDate)
                .FirstOrDefaultAsync();

            if (lc == null) return Ok(new { });

            return Ok(new
            {
                id = lc.Id,
                title = lc.Title,
                courseName = lc.Course?.Title ?? "",
                instructor = lc.Instructor?.FullName ?? "",
                roomId = lc.RoomId,
                scheduledDate = lc.ScheduledDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                time = lc.ScheduledDate.ToString("dd MMM, HH:mm"),
                isLive = lc.Status == LiveSessionStatus.Live ||
                         (lc.Status == LiveSessionStatus.Scheduled && DateTime.Now >= lc.ScheduledDate)
            });
        }
    }
}
