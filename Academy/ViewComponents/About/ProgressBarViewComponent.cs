using Microsoft.AspNetCore.Mvc;

namespace Academy.ViewComponents.About
{
    public class ProgressBarViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
