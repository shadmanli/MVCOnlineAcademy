using Microsoft.AspNetCore.Mvc;
using Academy.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Academy.Controllers
{
    [Route("meeting")]
    public class MeetingController : Controller
    {
        private readonly AppDbContext _context;

        public MeetingController(AppDbContext context)
        {
            _context = context;
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

        [HttpGet("{roomId}")]
        public async Task<IActionResult> Join(string roomId)
        {
            var liveClass = await _context.LiveClasses
                .Include(l => l.Course)
                .Include(l => l.Instructor)
                .FirstOrDefaultAsync(l => l.RoomId == roomId);

            if (liveClass == null) 
            {
                return View("NotFound");
            }

            return View(liveClass);
        }
    }
}
