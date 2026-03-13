using Microsoft.AspNetCore.Mvc;

namespace Academy.ViewComponents.Advance
{
    public class AdvanceViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
