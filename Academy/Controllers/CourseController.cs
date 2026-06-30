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

        // AJAX: kurs kartlarını page-ə görə gətir (refreshsiz pagination)
        [HttpGet]
        public IActionResult GetPage(
            string? search, string? sort, int? categoryId,
            int? instructorId, string? level,
            decimal? minPrice, decimal? maxPrice, int page = 1)
        {
            return ViewComponent("CourseCard", new
            {
                search, sort, categoryId, instructorId,
                level, minPrice, maxPrice, page
            });
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
