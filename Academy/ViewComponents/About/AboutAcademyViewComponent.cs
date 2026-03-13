using Microsoft.AspNetCore.Mvc;

namespace Academy.ViewComponents.About
{
    public class AboutAcademyViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
