using Academy.Data;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Academy.ViewComponents.About
{
    public class TestimonialViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public TestimonialViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var reviews = await _context.CourseReviews
                .Include(r => r.Course)
                .Where(r => r.Status == ReviewStatus.Approved)
                .OrderByDescending(r => r.Rating)
                .ThenByDescending(r => r.CreatedAt)
                .Take(8)
                .ToListAsync();

            return View(reviews);
        }
    }
}
