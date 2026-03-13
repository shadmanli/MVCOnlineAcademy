using Microsoft.AspNetCore.Mvc;

namespace Academy.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
