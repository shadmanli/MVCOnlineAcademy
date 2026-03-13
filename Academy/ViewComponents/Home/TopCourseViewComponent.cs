using Microsoft.AspNetCore.Mvc;

namespace Academy.ViewComponents.TopCourse
{
    public class TopCourseViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
