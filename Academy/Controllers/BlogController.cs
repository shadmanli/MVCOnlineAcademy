using Microsoft.AspNetCore.Mvc;

namespace Academy.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
