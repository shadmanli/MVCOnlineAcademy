using Academy.Data;
using Academy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Academy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin,Muellim")]
    [Route("admin/quiz")]
    public class QuizController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public QuizController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Admin bütün kurslar?n quizl?rin? baxa bilsin
        // Mü?llim yaln?z öz kursunun quiz-ini gör? bilsin
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var isMuellim = await _userManager.IsInRoleAsync(currentUser, "Muellim");
            var isAdmin = await _userManager.IsInRoleAsync(currentUser, "Admin") || await _userManager.IsInRoleAsync(currentUser, "SuperAdmin");

            var quizzesQuery = _context.Quizzes.Include(q => q.Course).AsQueryable();

            if (isMuellim && !isAdmin)
            {
                // Instructor ownership check - checking UserCourse or custom logic if Instructor entity is used
                // Assuming UserCourse exists for mapping instructors to courses, or Course has some link.
                // In absence of direct AppUserId on Course, we map by instructor Name or UserCourse:
                var myCourseIds = await _context.Set<UserCourse>()
                                        .Where(uc => uc.UserId == currentUser.Id)
                                        .Select(uc => uc.CourseId)
                                        .ToListAsync();
                quizzesQuery = quizzesQuery.Where(q => myCourseIds.Contains(q.CourseId));
            }

            var quizzes = await quizzesQuery.ToListAsync();
            return Json(quizzes); // Just returning JSON for backend logic representation. Can be View().
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(string title, int courseId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var isMuellim = await _userManager.IsInRoleAsync(currentUser, "Muellim");
            var isAdmin = await _userManager.IsInRoleAsync(currentUser, "Admin") || await _userManager.IsInRoleAsync(currentUser, "SuperAdmin");

            if (isMuellim && !isAdmin)
            {
                var myCourseIds = await _context.Set<UserCourse>()
                                        .Where(uc => uc.UserId == currentUser.Id)
                                        .Select(uc => uc.CourseId)
                                        .ToListAsync();
                if (!myCourseIds.Contains(courseId))
                    return StatusCode(403, "Bu kurs siz? aid deyil.");
            }

            // Bir kursa ikinci quiz ?lav? etm?y? çal??anda x?ta
            var exists = await _context.Quizzes.AnyAsync(q => q.CourseId == courseId);
            if (exists)
                return BadRequest("Bu kursun art?q quiz-i var");

            var quiz = new Quiz
            {
                Title = title,
                CourseId = courseId
            };

            _context.Quizzes.Add(quiz);
            await _context.SaveChangesAsync();

            return Ok(quiz);
        }

        [HttpPost("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var quiz = await _context.Quizzes.FindAsync(id);
            if (quiz == null) return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            var isMuellim = await _userManager.IsInRoleAsync(currentUser, "Muellim");
            var isAdmin = await _userManager.IsInRoleAsync(currentUser, "Admin") || await _userManager.IsInRoleAsync(currentUser, "SuperAdmin");

            if (isMuellim && !isAdmin)
            {
                var myCourseIds = await _context.Set<UserCourse>()
                                        .Where(uc => uc.UserId == currentUser.Id)
                                        .Select(uc => uc.CourseId)
                                        .ToListAsync();
                if (!myCourseIds.Contains(quiz.CourseId))
                    return StatusCode(403, "Bu kurs siz? aid deyil.");
            }

            // Quiz silin?nd? kurs silinm?sin (Cascade handling avoids it because Course is Principal)
            _context.Quizzes.Remove(quiz);
            await _context.SaveChangesAsync();

            return Ok("Silindi");
        }

        [HttpGet("results/{courseId}")]
        public async Task<IActionResult> Results(int courseId)
        {
            // Mü?llim t?l?b? n?tic?l?rini yaln?z öz kursuna gör? görsün
            var currentUser = await _userManager.GetUserAsync(User);
            var isMuellim = await _userManager.IsInRoleAsync(currentUser, "Muellim");
            var isAdmin = await _userManager.IsInRoleAsync(currentUser, "Admin") || await _userManager.IsInRoleAsync(currentUser, "SuperAdmin");

            if (isMuellim && !isAdmin)
            {
                var myCourseIds = await _context.Set<UserCourse>()
                                        .Where(uc => uc.UserId == currentUser.Id)
                                        .Select(uc => uc.CourseId)
                                        .ToListAsync();
                if (!myCourseIds.Contains(courseId))
                    return StatusCode(403, "Bu kurs siz? aid deyil.");
            }

            var results = await _context.UserAssessmentResults
                .Include(r => r.AppUser)
                .Where(r => r.CourseId == courseId)
                .ToListAsync();

            return Json(results);
        }
    }
}
