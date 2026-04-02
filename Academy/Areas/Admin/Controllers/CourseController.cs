using Academy.Data;
using Academy.Services.Interfaces;
using Academy.ViewModels.Course;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Academy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CourseController : Controller
    {
        private readonly ICourseService _service;
        private readonly AppDbContext _context;

        public CourseController(ICourseService service, AppDbContext context)
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
            var model = new CourseCreateVM
            {
                Languages = _context.Languages.Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList(),
                Categories = _context.Categories.Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList(),
                Instructors = _context.Instructors.Select(x => new SelectListItem(x.FullName, x.Id.ToString())).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CourseCreateVM model)
        {
            if (!ModelState.IsValid) return View(model);

            await _service.CreateAsync(model);
            return RedirectToAction(nameof(Index));
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
    }
}
