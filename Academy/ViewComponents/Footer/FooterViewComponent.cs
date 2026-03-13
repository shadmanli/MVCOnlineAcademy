using Microsoft.AspNetCore.Mvc;

namespace Academy.ViewComponents.Footer
{
    public class FooterViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
