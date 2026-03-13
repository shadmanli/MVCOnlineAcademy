using System.Diagnostics;

using Microsoft.AspNetCore.Mvc;

namespace Academy.Controllers
{
    public class HomeController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }

      
    }
}
