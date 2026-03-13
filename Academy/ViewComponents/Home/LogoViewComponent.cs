using Microsoft.AspNetCore.Mvc;

namespace Academy.ViewComponents.Home
{
    public class LogoViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
