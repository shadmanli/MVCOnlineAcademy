using Academy.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Academy.ViewComponents.TopCourse
{
    public class TopCourseViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public TopCourseViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // DB-dən aktiv kursları kateqoriya və müəllim ilə yüklə
            var courses = await _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Instructor)
                .Where(c => c.IsActive && !c.IsDeleted)
                .OrderByDescending(c => c.StudentCount)
                .Take(12)
                .ToListAsync();

            // Unikal kateqoriyalar
            var categories = courses
                .Where(c => c.Category != null)
                .Select(c => c.Category!)
                .GroupBy(c => c.Id)
                .Select(g => g.First())
                .OrderBy(c => c.Name)
                .ToList();

            ViewBag.Categories = categories;
            return View(courses);
        }
    }
}
