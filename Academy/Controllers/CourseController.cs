using Academy.Data;
using Academy.Services.Interfaces;
using Academy.ViewModels.Course;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Academy.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseService _service;
        private readonly AppDbContext _context;

        public CourseController(ICourseService service,AppDbContext context)
        {
            _service = service;
            _context = context;
        }

        // Kurs list
        public IActionResult Index()
        {
            return View();
        }

        // Detail səhifə
        public async Task<IActionResult> Detail(int id)
        {
            var course = await _service.GetByIdAsync(id);
            if (course == null) return NotFound();
            return View("~/Views/CourseDetail/Index.cshtml", course);
        }
    }
}
