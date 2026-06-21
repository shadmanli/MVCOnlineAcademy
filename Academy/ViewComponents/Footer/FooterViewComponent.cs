using Academy.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Academy.ViewComponents.Footer
{
    public class FooterViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public FooterViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // AboutUs-dan phone + address
            var aboutUs = await _context.AboutUs.FirstOrDefaultAsync();
            ViewBag.Phone   = aboutUs?.Phone;

            // ContactItems-dan description sahəsi üzərindən
            // Address, Email, Phone tipli məlumatlar
            var contactItems = await _context.ContactItems
                .Include(c => c.ContactSection)
                .ToListAsync();

            ViewBag.ContactItems = contactItems;

            return View();
        }
    }
}
