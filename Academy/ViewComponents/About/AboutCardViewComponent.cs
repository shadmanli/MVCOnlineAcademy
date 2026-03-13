using Microsoft.AspNetCore.Mvc;

namespace Academy.ViewComponents.About
{
    public class AboutCardViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    } 
}
