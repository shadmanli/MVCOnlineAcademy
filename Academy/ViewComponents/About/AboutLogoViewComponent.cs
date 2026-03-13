using Microsoft.AspNetCore.Mvc;

namespace Academy.ViewComponents.About
{
    public class AboutLogoViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
