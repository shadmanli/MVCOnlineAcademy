using Microsoft.AspNetCore.Mvc;

namespace Academy.ViewComponents.Contact
{
    public class ContactInfoViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
