using Academy.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Academy.ViewComponents.Home
{
    public class LogoViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public LogoViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var partners = await _context.Partners
                .Where(p => p.IsActive)
                .OrderBy(p => p.Order)
                .ThenBy(p => p.Id)
                .ToListAsync();

            return View(partners);
        }
    }
}
