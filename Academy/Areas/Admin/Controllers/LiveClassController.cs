using Academy.Data;
using Academy.Models;
using Academy.ViewModels.LiveClass;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Academy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin,Muellim")]
    public class LiveClassController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public LiveClassController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Admin/LiveClass
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var isMuellim = await _userManager.IsInRoleAsync(currentUser, "Muellim");
            var isAdminOrSuper = await _userManager.IsInRoleAsync(currentUser, "Admin") || await _userManager.IsInRoleAsync(currentUser, "SuperAdmin");

            var liveClassesQuery = _context.LiveClasses
                .Include(l => l.Course)
                .Include(l => l.Teacher)
                .AsQueryable();

            var liveClasses = await liveClassesQuery
                .OrderByDescending(l => l.ScheduledDate)
                .ToListAsync();

            ViewBag.CurrentUserId = currentUser.Id;
            ViewBag.IsMuellim = isMuellim;
            ViewBag.IsAdminOrSuper = isAdminOrSuper;

            return View(liveClasses);
        }

        // GET: Admin/LiveClass/GetLessons?courseId=5
        [HttpGet]
        public async Task<IActionResult> GetLessons(int courseId)
        {
            var lessons = await _context.Lessons
                .Where(l => l.CourseId == courseId)
                .Select(l => new { id = l.Id, title = l.Title })
                .ToListAsync();
            return Json(lessons);
        }

        // GET: Admin/LiveClass/GetCoursesByCategory?categoryId=3
        [HttpGet]
        public async Task<IActionResult> GetCoursesByCategory(int categoryId)
        {
            var courses = await _context.Courses
                .Where(c => c.CategoryId == categoryId)
                .Select(c => new { id = c.Id, title = c.Title })
                .ToListAsync();
            return Json(courses);
        }

        // GET: Admin/LiveClass/Create
        public async Task<IActionResult> Create()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var isAdminOrSuper = await _userManager.IsInRoleAsync(currentUser, "Admin") || await _userManager.IsInRoleAsync(currentUser, "SuperAdmin");

            var vm = new LiveClassCreateVM
            {
                Courses = await _context.Courses.Select(x => new SelectListItem(x.Title, x.Id.ToString())).ToListAsync(),
                ScheduledDate = DateTime.Now.AddDays(1),
                DurationMinutes = 60
            };

            ViewBag.Categories = await _context.Categories
                .Select(c => new SelectListItem(c.Name, c.Id.ToString()))
                .ToListAsync();

            if (isAdminOrSuper)
            {
                var teachers = await _userManager.GetUsersInRoleAsync("Muellim");
                vm.Teachers = teachers.Select(t => new SelectListItem(t.FullName ?? t.UserName, t.Id)).ToList();
            }

            return View(vm);
        }

        // POST: Admin/LiveClass/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LiveClassCreateVM model)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var isAdminOrSuper = await _userManager.IsInRoleAsync(currentUser, "Admin") || await _userManager.IsInRoleAsync(currentUser, "SuperAdmin");

            if (!ModelState.IsValid)
            {
                model.Courses = await _context.Courses.Select(x => new SelectListItem(x.Title, x.Id.ToString())).ToListAsync();
                if (isAdminOrSuper)
                {
                    var teachers = await _userManager.GetUsersInRoleAsync("Muellim");
                    model.Teachers = teachers.Select(t => new SelectListItem(t.FullName ?? t.UserName, t.Id)).ToList();
                }
                return View(model);
            }

            var teacherId = currentUser.Id;
            if (isAdminOrSuper && !string.IsNullOrEmpty(model.TeacherId))
            {
                teacherId = model.TeacherId;
            }

            var liveClass = new LiveClass
            {
                CourseId = model.CourseId,
                LessonId = model.LessonId,
                TeacherId = teacherId,
                Title = model.Title,
                Topic = model.Topic ?? "Mövzu qeyd edilməyib",
                ScheduledDate = DateTime.SpecifyKind(model.ScheduledDate, DateTimeKind.Unspecified),
                DurationMinutes = model.DurationMinutes,
                Status = LiveSessionStatus.Scheduled,
                RoomId = Guid.NewGuid().ToString("N"),
                SecureToken = Guid.NewGuid().ToString("N")
            };

            await _context.LiveClasses.AddAsync(liveClass);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/LiveClass/Edit
        public async Task<IActionResult> Edit(int id)
        {
            var liveClass = await _context.LiveClasses.FindAsync(id);
            if (liveClass == null) return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            var isMuellim = await _userManager.IsInRoleAsync(currentUser, "Muellim");
            var isAdminOrSuper = await _userManager.IsInRoleAsync(currentUser, "Admin") || await _userManager.IsInRoleAsync(currentUser, "SuperAdmin");

            if (isMuellim && !isAdminOrSuper)
            {
                if (liveClass.TeacherId != currentUser.Id)
                {
                    return Forbid();
                }
            }

            var vm = new LiveClassEditVM
            {
                Id = liveClass.Id,
                CourseId = liveClass.CourseId,
                Title = liveClass.Title,
                Topic = liveClass.Topic,
                ScheduledDate = liveClass.ScheduledDate,
                DurationMinutes = liveClass.DurationMinutes,
                Courses = await _context.Courses.Select(x => new SelectListItem(x.Title, x.Id.ToString())).ToListAsync()
            };

            return View(vm);
        }

        // POST: Admin/LiveClass/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LiveClassEditVM model)
        {
            if (!ModelState.IsValid)
            {
                model.Courses = await _context.Courses.Select(x => new SelectListItem(x.Title, x.Id.ToString())).ToListAsync();
                return View(model);
            }

            var liveClass = await _context.LiveClasses.FindAsync(model.Id);
            if (liveClass == null) return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            var isMuellim = await _userManager.IsInRoleAsync(currentUser, "Muellim");
            var isAdminOrSuper = await _userManager.IsInRoleAsync(currentUser, "Admin") || await _userManager.IsInRoleAsync(currentUser, "SuperAdmin");

            if (isMuellim && !isAdminOrSuper)
            {
                if (liveClass.TeacherId != currentUser.Id)
                {
                    return Forbid();
                }
            }

            liveClass.CourseId = model.CourseId;
            liveClass.Title = model.Title;
            liveClass.Topic = model.Topic ?? "M�vzu qeyd edilm?yib";
            liveClass.ScheduledDate = DateTime.SpecifyKind(model.ScheduledDate, DateTimeKind.Unspecified);
            liveClass.DurationMinutes = model.DurationMinutes;

            _context.LiveClasses.Update(liveClass);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/LiveClass/Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var liveClass = await _context.LiveClasses.FindAsync(id);
            if (liveClass == null) return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            var isMuellim = await _userManager.IsInRoleAsync(currentUser, "Muellim");
            var isAdminOrSuper = await _userManager.IsInRoleAsync(currentUser, "Admin") || await _userManager.IsInRoleAsync(currentUser, "SuperAdmin");

            if (isMuellim && !isAdminOrSuper)
            {
                if (liveClass.TeacherId != currentUser.Id)
                {
                    return Forbid();
                }
            }

            _context.LiveClasses.Remove(liveClass);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
