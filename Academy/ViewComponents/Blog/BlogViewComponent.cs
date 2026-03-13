using Microsoft.AspNetCore.Mvc;

namespace Academy.ViewComponents.Blog
{
    public class BlogViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
