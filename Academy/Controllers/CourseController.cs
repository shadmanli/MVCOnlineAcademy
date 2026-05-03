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
            var course = await _context.Courses
                .Where(x => x.Id == id)
                .Select(x => new CourseDetailVM
                {
                    Title = x.Title,
                    Description = x.Description,
                    Price = x.Price,
                    ImageUrl = x.ImageUrl,
                    LanguageName = x.Language.Name,
                    CategoryName = x.Category.Name,
                    InstructorName = x.Instructor.FullName,
                    Duration = x.Duration
                })
                .FirstOrDefaultAsync();

            return View("~/Views/CourseDetail/Index.cshtml", course);
        }
    }
}
