using Microsoft.AspNetCore.Mvc;

namespace Academy.ViewComponents.Home
{
    public class NumbersViewComponent:ViewComponent
    {
public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
