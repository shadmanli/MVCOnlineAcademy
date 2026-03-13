using Microsoft.AspNetCore.Mvc;

namespace Academy.ViewComponents.Home
{
    public class TrendingCategoriesViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
