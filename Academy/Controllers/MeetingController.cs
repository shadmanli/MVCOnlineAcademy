using Microsoft.AspNetCore.Mvc;
using Academy.Data;
using Academy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Academy.Controllers
{
    [Route("meeting")]
    [Authorize]
    public class MeetingController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public MeetingController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        [HttpGet("index")]
        public async Task<IActionResult> Index()
        {
            var liveClasses = await _context.LiveClasses
                .Include(l => l.Course)
                .Include(l => l.Instructor)
                .ToListAsync();
            return View(liveClasses);
        }

        [HttpGet("zoom")]
        public IActionResult Zoom()
        {
            // Placeholder for actual SDK or external zoom url wrapper
            return Content("Zoom SDK Integration Pending");
        }

        [HttpGet("{roomId}")]
        public async Task<IActionResult> Join(string roomId)
        {
            if(roomId.ToLower() == "zoom") return Zoom();

            var liveClass = await _context.LiveClasses
                .Include(l => l.Course)
                .Include(l => l.Instructor)
                .FirstOrDefaultAsync(l => l.RoomId == roomId);

            if (liveClass == null) 
            {
                return View("NotFound");
            }

            var currentUser = await _userManager.GetUserAsync(User);
            ViewBag.CurrentUserId = currentUser.Id;
            ViewBag.CurrentUserName = currentUser.UserName;
            ViewBag.CurrentFullName = currentUser.FullName ?? currentUser.UserName;
            ViewBag.IsTeacher = currentUser.Id == liveClass.TeacherId;

            return View(liveClass);
        }
    }
}
