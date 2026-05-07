using Microsoft.AspNetCore.Mvc;

namespace Academy.Controllers
{
    public class WishlistController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
