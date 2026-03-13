using Microsoft.AspNetCore.Mvc;

namespace Academy.ViewComponents.Header
{
    public class HeaderViewComponent:ViewComponent
    {
       public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
