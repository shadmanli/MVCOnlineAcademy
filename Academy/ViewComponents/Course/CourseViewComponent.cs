using Microsoft.AspNetCore.Mvc;

namespace Academy.ViewComponents.Course
{
    public class CourseViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
