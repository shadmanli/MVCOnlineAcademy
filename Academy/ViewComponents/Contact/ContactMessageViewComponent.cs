using Microsoft.AspNetCore.Mvc;

namespace Academy.ViewComponents.Contact
{
    public class ContactMessageViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
