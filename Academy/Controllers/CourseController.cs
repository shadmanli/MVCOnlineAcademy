using Microsoft.AspNetCore.Mvc;

namespace Academy.Controllers
{
    public class CourseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
