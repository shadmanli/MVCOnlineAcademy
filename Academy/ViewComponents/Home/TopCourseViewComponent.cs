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
                .Where(c => !c.IsDeleted) // Removed IsActive check so newly created courses can be displayed
                .OrderByDescending(c => c.Id)
                .Take(3)
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
