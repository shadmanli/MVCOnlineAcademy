using Academy.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseService _service;

        public CourseController(ICourseService service)
        {
            _service = service;
        }

        // Kurs list
        public async Task<IActionResult> Index()
        {
            var courses = await _service.GetAllAsync();
            return View(courses);
        }

        // Detail səhifə
        public async Task<IActionResult> Detail(int id)
        {
            var course = await _service.GetByIdAsync(id);

            if (course == null)
                return NotFound();

            return View(course);
        }
    }
}
