using Microsoft.AspNetCore.Mvc;

namespace Academy.ViewComponents.Home
{
    public class FeedbackViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
