using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Academy.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Academy.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SuperAdmin,Admin,Muellim")]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var results = await _context.UserAssessmentResults
                .Include(r => r.AppUser)
                .Include(r => r.Category)
                .ToListAsync();

            ViewBag.TotalStudents = results.Select(r => r.AppUserId).Distinct().Count();
            ViewBag.BeginnerCount = results.Count(r => r.Level == "Beginner");
            ViewBag.IntermediateCount = results.Count(r => r.Level == "Intermediate");
            ViewBag.AdvancedCount = results.Count(r => r.Level == "Advanced");
            
            ViewBag.AverageScore = results.Any() ? results.Average(r => r.Percentage) : 0;
            ViewBag.TopStudent = results.OrderByDescending(r => r.Percentage).FirstOrDefault();

            return View(results.OrderByDescending(r => r.Percentage).ToList());
        }
    }
}
