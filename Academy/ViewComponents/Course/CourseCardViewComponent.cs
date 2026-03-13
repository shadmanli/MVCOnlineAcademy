using Microsoft.AspNetCore.Mvc;

namespace Academy.ViewComponents.Course
{
    public class CourseCardViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
