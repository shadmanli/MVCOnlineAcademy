using Microsoft.AspNetCore.Mvc;

namespace Academy.ViewComponents.Teacher
{
    public class TeacherBenefitViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
