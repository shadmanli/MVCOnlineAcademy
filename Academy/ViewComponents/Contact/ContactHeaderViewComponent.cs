using Microsoft.AspNetCore.Mvc;

namespace Academy.ViewComponents.Contact
{
    public class ContactHeaderViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
