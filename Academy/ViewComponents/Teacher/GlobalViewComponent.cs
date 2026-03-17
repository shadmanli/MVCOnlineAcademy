using Microsoft.AspNetCore.Mvc;

namespace Academy.ViewComponents.Teacher
{
    public class GlobalViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
