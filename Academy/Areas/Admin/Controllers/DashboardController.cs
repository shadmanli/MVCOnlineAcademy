using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Academy.Data;
using Microsoft.AspNetCore.Identity;
using Academy.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Academy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SuperAdmin,Admin,Muellim")]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public DashboardController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // Admin, SuperAdmin, Muellim rollarındakı user ID-lərini tap
            var adminRole  = await _userManager.GetUsersInRoleAsync("Admin");
            var superRole  = await _userManager.GetUsersInRoleAsync("SuperAdmin");
            var muellimRole = await _userManager.GetUsersInRoleAsync("Muellim");

            var excludedIds = adminRole.Select(u => u.Id)
                .Union(superRole.Select(u => u.Id))
                .Union(muellimRole.Select(u => u.Id))
                .ToHashSet();

            // Yalnız tələbə (User rolu) nəticələrini göstər
            var results = await _context.UserAssessmentResults
                .Include(r => r.AppUser)
                .Include(r => r.Category)
                .Where(r => !excludedIds.Contains(r.AppUserId))
                .OrderByDescending(r => r.Percentage)
                .ToListAsync();

            ViewBag.TotalStudents     = results.Count;
            ViewBag.BeginnerCount     = results.Count(r => r.Level == "Beginner");
            ViewBag.IntermediateCount = results.Count(r => r.Level == "Intermediate");
            ViewBag.AdvancedCount     = results.Count(r => r.Level == "Advanced");
            ViewBag.AverageScore      = results.Any() ? results.Average(r => r.Percentage) : 0;

            return View(results);
        }
    }
}
