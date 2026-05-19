using Microsoft.AspNetCore.Mvc;
using Academy.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Academy.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Academy.Controllers
{
    public class AssessmentController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public AssessmentController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index(int? categoryId)
        {
            ViewBag.CategoryId = categoryId;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetQuestions(int categoryId)
        {
            var questionsQuery = _context.AssessmentQuestions
                .Include(q => q.Options)
                .AsQueryable();

            if (categoryId > 0)
            {
                questionsQuery = questionsQuery.Where(q => q.CategoryId == categoryId);
            }

            var questions = await questionsQuery.ToListAsync();

            if (questions == null || !questions.Any())
                return NotFound();

            var rng = new Random();
            
            // Anti-repeat/Randomize: Shuffle questions and take max 15
            var shuffledQuestions = questions.OrderBy(x => rng.Next()).Take(15).Select(q => new
            {
                id = q.Id,
                text = q.Text,
                // Randomize options for each question
                options = q.Options.OrderBy(x => rng.Next()).Select(o => new { id = o.Id, text = o.Text }).ToList()
            }).ToList();

            return Json(shuffledQuestions);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitAssessment([FromBody] AssessmentSubmissionDto submission)
        {
            if (submission == null || submission.Answers == null || submission.CategoryId == 0)
                return BadRequest();

            int correctCount = 0;
            int total = submission.Answers.Count;

            // Score evaluation backend-side (Secure)
            foreach (var answer in submission.Answers)
            {
                var isCorrect = await _context.AssessmentOptions
                    .Where(o => o.Id == answer.OptionId && o.AssessmentQuestionId == answer.QuestionId)
                    .Select(o => o.IsCorrect)
                    .FirstOrDefaultAsync();

                if (isCorrect) correctCount++;
            }

            double percentage = total == 0 ? 0 : (double)correctCount / total;
            string level = "Beginner";
            int xp = correctCount * 50;

            // Professional Level Classification Logic
            if (percentage >= 0.8) level = "Advanced";
            else if (percentage >= 0.4) level = "Intermediate";

            // Update user in DB
            if (User.Identity.IsAuthenticated)
            {
                var appUser = await _userManager.GetUserAsync(User);
                if (appUser != null)
                {
                    var existingResult = await _context.UserAssessmentResults
                        .FirstOrDefaultAsync(r => r.AppUserId == appUser.Id && r.CategoryId == submission.CategoryId);

                    if (existingResult != null)
                    {
                        existingResult.Score = correctCount;
                        existingResult.TotalQuestions = total;
                        existingResult.Percentage = percentage * 100;
                        existingResult.Level = level;
                        existingResult.XP = xp;
                    }
                    else
                    {
                        var newResult = new UserAssessmentResult
                        {
                            AppUserId = appUser.Id,
                            CategoryId = submission.CategoryId,
                            Score = correctCount,
                            TotalQuestions = total,
                            Percentage = percentage * 100,
                            Level = level,
                            XP = xp
                        };
                        _context.UserAssessmentResults.Add(newResult);
                    }
                    await _context.SaveChangesAsync();
                }
            }

            // Generate Recommendations based on calculated Level
            string roadmap = level == "Beginner" 
                ? "T?m?l proqramla?d?rma anlay??lar?, Syntax, OOP prinsipl?ri üzr? baza yarad?n." 
                : level == "Intermediate" 
                ? "Data Structures, LINQ, Asinxron proqramla?d?rma v? Database arxitekturas?n? d?rind?n öyr?nin." 
                : "System Design, Microservices, Cloud Architecture (Azure/AWS) v? Design Patterns t?tbiq edin.";

            string strengths = level == "Advanced" ? "Analitik dü?ünc?, Kompleks M?ntiq" : level == "Intermediate" ? "Baza bilikl?ri, Alqoritmik yana?ma" : "Öyr?nm?y? h?v?s, T?m?l m?ntiq";
            string weaknesses = level == "Advanced" ? "Sistem Arxitekturas? (B?lk?)" : level == "Intermediate" ? "Performance Optimization" : "D?rin OOP Prinsipl?ri, Design Patterns";

            return Json(new
            {
                level = level,
                score = correctCount,
                total = total,
                percentage = Math.Round(percentage * 100),
                xp = xp,
                roadmap = roadmap,
                strengths = strengths,
                weaknesses = weaknesses
            });
        }
    }

    public class AssessmentSubmissionDto
    {
        public int CategoryId { get; set; }
        public List<AnswerDto> Answers { get; set; } = new List<AnswerDto>();
    }

    public class AnswerDto
    {
        public int QuestionId { get; set; }
        public int OptionId { get; set; }
    }
}