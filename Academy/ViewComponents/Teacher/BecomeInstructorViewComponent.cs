using Microsoft.AspNetCore.Mvc;

namespace Academy.ViewComponents.Teacher
{
    public class BecomeInstructorViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
