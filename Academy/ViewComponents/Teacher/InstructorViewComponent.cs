using Microsoft.AspNetCore.Mvc;

namespace Academy.ViewComponents.Teacher
{
    public class InstructorViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
