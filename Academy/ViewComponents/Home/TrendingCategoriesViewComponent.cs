using Academy.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Academy.ViewComponents.Home
{
    public class TrendingCategoryVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CourseCount { get; set; }
    }

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
                .Select(c => new TrendingCategoryVM
                {
                    Id         = c.Id,
                    Name       = c.Name,
                    CourseCount = c.Courses.Count(co => !co.IsDeleted)
                })
                .OrderByDescending(c => c.CourseCount)
                .ToListAsync();

            return View(categories);
        }
    }
}
