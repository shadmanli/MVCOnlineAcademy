using Academy.Data;
using Academy.Models;
using Academy.ViewModels.LiveClass;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Academy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LiveClassController : Controller
    {
        private readonly AppDbContext _context;

        public LiveClassController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/LiveClass
        public async Task<IActionResult> Index()
        {
            var liveClasses = await _context.LiveClasses
                .Include(l => l.Course)
                .Include(l => l.Instructor)
                .OrderByDescending(l => l.ScheduledDate)
                .ToListAsync();

            return View(liveClasses);
        }

        // GET: Admin/LiveClass/Create
        public async Task<IActionResult> Create()
        {
            var vm = new LiveClassCreateVM
            {
                Courses = await _context.Courses.Select(x => new SelectListItem(x.Title, x.Id.ToString())).ToListAsync(),
                Instructors = await _context.Instructors.Select(x => new SelectListItem(x.FullName, x.Id.ToString())).ToListAsync(),
                ScheduledDate = DateTime.Now.AddDays(1),
                DurationMinutes = 60
            };
            return View(vm);
        }

        // POST: Admin/LiveClass/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LiveClassCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                model.Courses = await _context.Courses.Select(x => new SelectListItem(x.Title, x.Id.ToString())).ToListAsync();
                model.Instructors = await _context.Instructors.Select(x => new SelectListItem(x.FullName, x.Id.ToString())).ToListAsync();
                return View(model);
            }

            var liveClass = new LiveClass
            {
                CourseId = model.CourseId,
                InstructorId = model.InstructorId,
                Title = model.Title,
                Topic = model.Topic ?? "Mövzu qeyd edilm?yib",
                ScheduledDate = model.ScheduledDate,
                DurationMinutes = model.DurationMinutes,
                Status = LiveSessionStatus.Scheduled,
                RoomId = Guid.NewGuid().ToString("N"),
                SecureToken = Guid.NewGuid().ToString("N") // Default token
            };

            await _context.LiveClasses.AddAsync(liveClass);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/LiveClass/Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var liveClass = await _context.LiveClasses.FindAsync(id);
            if (liveClass == null) return NotFound();

            _context.LiveClasses.Remove(liveClass);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
