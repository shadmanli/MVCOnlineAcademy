using Academy.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Controllers
{
    public class TeacherController : Controller
    {
        private readonly IInstructorService _instructorService;

        public TeacherController(IInstructorService instructorService)
        {
            _instructorService = instructorService;
        }

        public IActionResult Index()
        {
            return View();
        }

        // AJAX: müəllimləri page-ə görə gətir
        [HttpGet]
        public async Task<IActionResult> GetPage(int page = 1)
        {
            var all = (await _instructorService.GetAllAsync()).ToList();
            const int pageSize = 8;
            int totalPages = (int)Math.Ceiling(all.Count / (double)pageSize);
            var paged = all.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages  = totalPages;

            return PartialView("_InstructorPagePartial", paged);
        }
    }
}
