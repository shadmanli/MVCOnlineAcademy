using Microsoft.AspNetCore.Mvc;

namespace Academy.ViewComponents.About
{
    public class OpportunitiesViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
