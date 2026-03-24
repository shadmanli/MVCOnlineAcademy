using Academy.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Academy.ViewComponents.Contact
{
    public class ContactInfoViewComponent : ViewComponent
    {
        private readonly IContactSectionService _contactSectionService;

        public ContactInfoViewComponent(IContactSectionService contactSectionService)
        {
            _contactSectionService = contactSectionService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var data = await _contactSectionService.GetAllAsync();
            return View(data);
        }
    }
}
