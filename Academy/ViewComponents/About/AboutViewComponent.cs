using Microsoft.AspNetCore.Mvc;

namespace Academy.ViewComponents.About
{
    public class AboutViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
