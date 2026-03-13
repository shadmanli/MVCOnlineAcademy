using Microsoft.AspNetCore.Mvc;

namespace Academy.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
