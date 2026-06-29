using Academy.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Academy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin,Muellim")]
    public class AdminSearchController : Controller
    {
        private readonly AppDbContext _context;

        public AdminSearchController(AppDbContext context)
        {
            _context = context;
        }

        // GET /Admin/AdminSearch/Search?q=...
        [HttpGet]
        public async Task<IActionResult> Search(string q)
        {
            if (string.IsNullOrWhiteSpace(q) || q.Length < 2)
                return Json(new List<object>());

            q = q.Trim().ToLower();
            var results = new List<object>();

            // ── Kurslar ──────────────────────────────
            var courses = await _context.Courses
                .Include(c => c.Category)
                .Where(c => c.Title.ToLower().Contains(q) && !c.IsDeleted)
                .Take(5)
                .ToListAsync();

            results.AddRange(courses.Select(c => new
            {
                type     = "Kurs",
                title    = c.Title,
                subtitle = c.Category?.Name ?? "",
                url      = $"/Admin/Course/Edit/{c.Id}"
            }));

            // ── Müəllimlər ───────────────────────────
            var instructors = await _context.Instructors
                .Where(i => i.FullName != null && i.FullName.ToLower().Contains(q))
                .Take(4)
                .ToListAsync();

            results.AddRange(instructors.Select(i => new
            {
                type     = "Müəllim",
                title    = i.FullName,
                subtitle = i.Title ?? "",
                url      = $"/Admin/Instructor/Edit/{i.Id}"
            }));

            // ── Tələbələr ────────────────────────────
            var students = await _context.Users
                .Where(u => (u.FullName != null && u.FullName.ToLower().Contains(q)) ||
                            (u.Email != null && u.Email.ToLower().Contains(q)))
                .Take(4)
                .ToListAsync();

            // Yalnız User rolunda olanlar
            var userRoleId = await _context.Roles
                .Where(r => r.Name == "User")
                .Select(r => r.Id)
                .FirstOrDefaultAsync();

            if (userRoleId != null)
            {
                var studentUserIds = await _context.UserRoles
                    .Where(ur => ur.RoleId == userRoleId)
                    .Select(ur => ur.UserId)
                    .ToListAsync();

                var filteredStudents = students.Where(s => studentUserIds.Contains(s.Id)).ToList();

                results.AddRange(filteredStudents.Select(s => new
                {
                    type     = "Tələbə",
                    title    = s.FullName ?? s.Email ?? "",
                    subtitle = s.Email ?? "",
                    url      = "/Admin/DashBoard/Index"
                }));
            }

            // ── Blog ─────────────────────────────────
            var blogs = await _context.Blog
                .Where(b => (b.Title != null && b.Title.ToLower().Contains(q)) ||
                            (b.Description != null && b.Description.ToLower().Contains(q)))
                .Take(4)
                .ToListAsync();

            results.AddRange(blogs.Select(b => new
            {
                type     = "Blog",
                title    = b.Title,
                subtitle = b.Name ?? "",
                url      = $"/Admin/Blog/Edit/{b.Id}"
            }));

            // ── Kateqoriyalar ─────────────────────────
            var cats = await _context.Categories
                .Where(c => c.Name.ToLower().Contains(q))
                .Take(4)
                .ToListAsync();

            results.AddRange(cats.Select(c => new
            {
                type     = "Kateqoriya",
                title    = c.Name,
                subtitle = $"{c.Courses?.Count ?? 0} kurs",
                url      = $"/Admin/Category/Edit/{c.Id}"
            }));

            return Json(results);
        }
    }
}
