using Microsoft.AspNetCore.Mvc;

namespace Academy.ViewComponents.Teacher
{
    public class SayViewComponent:ViewComponent

    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
