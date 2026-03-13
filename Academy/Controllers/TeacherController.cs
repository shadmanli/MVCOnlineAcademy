using Microsoft.AspNetCore.Mvc;

namespace Academy.Controllers
{
    public class TeacherController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
