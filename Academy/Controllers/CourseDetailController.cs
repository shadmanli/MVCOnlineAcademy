using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Academy.Services.Interfaces;

namespace Academy.Controllers
{
    public class CourseDetailController : Controller
    {
        private readonly ICourseService _courseService;

        public CourseDetailController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public async Task<IActionResult> Index(int id)
        {
            var course = await _courseService.GetByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }
    }
}
