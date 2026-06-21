using Academy.Data;
using Academy.Models;
using Academy.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Academy.ViewComponents.Home
{
    public class FeedbackViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public FeedbackViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var features = await _context.Feature
                .OrderBy(f => f.Id)
                .Take(2)
                .ToListAsync();

            // Birbaşa Statistics cədvəlindən al
            var statistics = await _context.Statistics
                .OrderBy(s => s.Id)
                .Take(3)
                .ToListAsync();

            ViewBag.Statistics = statistics;

            return View(features);
        }
    }
}
