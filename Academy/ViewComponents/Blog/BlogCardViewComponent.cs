using Microsoft.AspNetCore.Mvc;

namespace Academy.ViewComponents.Blog
{
    public class BlogCardViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
