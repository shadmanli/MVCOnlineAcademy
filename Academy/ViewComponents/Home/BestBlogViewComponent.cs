using Microsoft.AspNetCore.Mvc;

namespace Academy.ViewComponents.Home
{
    public class BestBlogViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
