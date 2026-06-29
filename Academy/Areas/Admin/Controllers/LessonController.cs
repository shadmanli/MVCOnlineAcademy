using Academy.Data;
using Academy.Services.Interfaces;
using Academy.ViewModels.Lesson;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Academy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SuperAdmin,Admin,Muellim")]
    public class LessonController : Controller
    {
        private readonly ILessonService _service;
        private readonly AppDbContext _context;

        public LessonController(ILessonService service, AppDbContext context)
        {
            _service = service;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _service.GetAllAsync());
        }

        public async Task<IActionResult> Create()
        {
            var model = new LessonCreateVM
            {
                Courses = new List<SelectListItem>()
            };

            ViewBag.Categories = await _context.Categories
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToListAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(LessonCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                model.Courses = await _context.Courses
                    .Select(x => new SelectListItem
                    {
                        Text = x.Title,
                        Value = x.Id.ToString()
                    }).ToListAsync();

                ViewBag.Categories = await _context.Categories
                    .Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }).ToListAsync();

                return View(model);
            }

            await _service.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        // AJAX: kateqoriyaya görə kursları gətir
        [HttpGet]
        public async Task<IActionResult> GetCoursesByCategory(int categoryId)
        {
            var courses = await _context.Courses
                .Where(c => c.CategoryId == categoryId)
                .Select(c => new { id = c.Id, title = c.Title })
                .ToListAsync();
            return Json(courses);
        }

 

        public async Task<IActionResult> Detail(int id)
        {
            return View(await _service.GetByIdAsync(id));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }



        public async Task<IActionResult> Edit(int id)
        {
            var data = await _service.GetEditByIdAsync(id);
            if (data == null) return NotFound();

            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(LessonEditVM model)
        {
            await _service.EditAsync(model);
            return RedirectToAction(nameof(Index));
        }
    }
}
