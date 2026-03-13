using Microsoft.AspNetCore.Mvc;

namespace Academy.ViewComponents.About
{
    public class TestimonialViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
