using Academy.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Academy.ViewComponents.Home
{
    public class TrendingCategoriesViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public TrendingCategoriesViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Kateqoriyaları kurs sayı ilə birlikdə yüklə
            var categories = await _context.Categories
                .Select(c => new
                {
                    Id         = c.Id,
                    Name       = c.Name,
                    CourseCount = c.Courses.Count(co => co.IsActive && !co.IsDeleted)
                })
                .OrderByDescending(c => c.CourseCount)
                .ToListAsync();

            // anonymous type → tuple list kimi ötür
            var result = categories
                .Select(c => (c.Id, c.Name, c.CourseCount))
                .ToList();

            return View(result);
        }
    }
}
