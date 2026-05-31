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
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> GetQuestions(int courseId)
        {
            var quiz = await _context.Quizzes
                .Include(q => q.Questions)
                .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(q => q.CourseId == courseId);

            if (quiz == null || !quiz.Questions.Any())
                return NotFound();

            var rng = new Random();

            // Anti-repeat/Randomize: Shuffle questions and take max 15
            var shuffledQuestions = quiz.Questions.OrderBy(x => rng.Next()).Take(15).Select(q => new
            {
                id = q.Id,
                text = q.Text,
                difficulty = q.Difficulty,
                points = q.Points,
                // Randomize options for each question
                options = q.Options.OrderBy(x => rng.Next()).Select(o => new { id = o.Id, text = o.Text }).ToList()
            }).ToList();

            return Json(shuffledQuestions);
        }

        [HttpPost]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> SubmitAssessment([FromBody] AssessmentSubmissionDto submission)
        {
            if (submission == null || submission.Answers == null || !submission.Answers.Any())
                return BadRequest("Cavablar bo? ola bilm?z.");

            if (!submission.CourseId.HasValue)
                return BadRequest("CourseId m�tl?qdir.");

            var quiz = await _context.Quizzes.FirstOrDefaultAsync(q => q.CourseId == submission.CourseId.Value);
            if (quiz == null)
                return BadRequest("Bu kursun quiz-i yoxdur.");

            int correctCount = 0;
            int total = submission.Answers.Count;
            double finalScore = 0;
            int wrongCount = 0;
            
            // We need Combo Bonus. Let's assume frontend sends answers in order of submission for real combo, or we just calculate overall combo if sequential
            // Actually, usually frontend tracks Combo and Speed, but since we are doing it securely backend-side, 
            // the DTO needs to provide TimeTaken per question, and we evaluate them in order.
            
            int currentCombo = 0;

            foreach (var answer in submission.Answers)
            {
                var question = await _context.AssessmentQuestions
                    .Include(q => q.Options)
                    .FirstOrDefaultAsync(q => q.Id == answer.QuestionId);

                if (question == null) continue;

                var isCorrect = question.Options
                    .Where(o => o.Id == answer.OptionId)
                    .Select(o => o.IsCorrect)
                    .FirstOrDefault();

                if (isCorrect) 
                {
                    correctCount++;
                    currentCombo++;
                    
                    double basePoints = question.Difficulty == DifficultyLevel.Easy ? 1 :
                                        question.Difficulty == DifficultyLevel.Medium ? 2 :
                                        question.Difficulty == DifficultyLevel.Hard ? 3 : 1;
                    
                    double questionScore = basePoints;
                    
                    // SpeedBonus logic: if TimeTaken is fast enough (e.g., < 10s gives extra 10%)
                    if (answer.TimeTakenSeconds > 0 && answer.TimeTakenSeconds <= 10)
                    {
                        questionScore += 0.5; // Speed bonus
                    }
                    
                    // ComboBonus logic: currentCombo > 2 gives +0.5 multiplier per extra streak (capped)
                    if (currentCombo >= 3)
                    {
                        questionScore += 0.5;
                    }

                    finalScore += questionScore;
                }
                else
                {
                    wrongCount++;
                    currentCombo = 0;
                   
                    finalScore -= 0.5;
                }
            }

            if (finalScore < 0) finalScore = 0;

            double percentage = total == 0 ? 0 : (double)correctCount / total;

            double percentageForLevel = percentage * 100;
            string level = "Beginner";

            if (percentageForLevel >= 71) level = "Advanced";
            else if (percentageForLevel >= 41) level = "Intermediate";

            int xp = (int)Math.Round(finalScore * 50);

            AppUser appUser = await _userManager.GetUserAsync(User);
            if (appUser != null)
            {
                // CategoryId-ni quiz suallarından tap
                int categoryId = 0;
                if (submission.CategoryId.HasValue && submission.CategoryId > 0)
                {
                    categoryId = submission.CategoryId.Value;
                }
                else
                {
                    // Cavablardan birinin kateqoriyasını tap
                    var firstQuestionId = submission.Answers.FirstOrDefault()?.QuestionId ?? 0;
                    if (firstQuestionId > 0)
                    {
                        var firstQ = await _context.AssessmentQuestions
                            .FirstOrDefaultAsync(q => q.Id == firstQuestionId);
                        categoryId = firstQ?.CategoryId ?? 0;
                    }
                }

                // Əgər hələ də 0-dırsa, kursun kateqoriyasını götür
                if (categoryId == 0 && submission.CourseId.HasValue)
                {
                    var course = await _context.Courses.FindAsync(submission.CourseId.Value);
                    categoryId = course?.CategoryId ?? 0;
                }

                // Son çarə — ilk kateqoriyanı götür
                if (categoryId == 0)
                {
                    var firstCat = await _context.Categories.FirstOrDefaultAsync();
                    categoryId = firstCat?.Id ?? 1;
                }

                var newResult = new UserAssessmentResult
                {
                    AppUserId = appUser.Id,
                    CategoryId = categoryId,
                    CourseId = submission.CourseId,
                    QuizId = quiz.Id,
                    Score = (int)Math.Round(finalScore),
                    TotalQuestions = total,
                    Percentage = percentageForLevel,
                    Level = level,
                    XP = xp
                };
                _context.UserAssessmentResults.Add(newResult);
                await _context.SaveChangesAsync();
            }

            // Create Recommendation System
            var wrongQuestionIds = submission.Answers.Where(a => 
            {
                var isCorrect = _context.AssessmentOptions.Where(o => o.Id == a.OptionId).Select(o => o.IsCorrect).FirstOrDefault();
                return !isCorrect;
            }).Select(a => a.QuestionId).ToList();

            var wrongCategories = await _context.AssessmentQuestions
                                        .Where(q => wrongQuestionIds.Contains(q.Id) && q.CategoryId > 0)
                                        .Select(q => q.CategoryId)
                                        .Distinct()
                                        .ToListAsync();

            if (submission.CategoryId.HasValue && submission.CategoryId > 0)
                wrongCategories.Add(submission.CategoryId.Value);

            // Fetch Courses dynamically based on performance
            var recommendationQuery = _context.Courses
                .Include(c => c.Category)
                .Where(c => c.IsActive && !c.IsDeleted && c.Id != submission.CourseId)
                .AsQueryable();

            var allCourses = await recommendationQuery.ToListAsync();

            // Simple recommendation ranking
            var rankedCourses = allCourses.Select(c => 
            {
                int score = 0;
                // Boost if course matches weak topic
                if (wrongCategories.Contains(c.CategoryId)) score += 10;
                
                // Boost based on title/level match
                if (level == c.Level) score += 20;
                
                return new { Course = c, Score = score };
            })
            // YALNIZ ger?k uy?unlu?u olan xsusi kurslar? qaytar (Score > 0). 
            // ?g?r uy?unluq yoxdursa siyah? bo? qals?n ki, UI-da "tap?lmad?" funksionall??? g�r�ns�n.
            .Where(x => x.Score > 0)
            .OrderByDescending(x => x.Score)
            .Take(4)
            .Select(x => x.Course)
            .ToList();

            var recommendedCourses = rankedCourses.Select(c => new
            {
                title = c.Title,
                imageUrl = c.ImageUrl, // Assuming ImageUrl is needed for UI
                url = Url.Action("Index", "CourseDetail", new { id = c.Id }), // Getting URL to course
                shortDescription = !string.IsNullOrEmpty(c.Description) && c.Description.Length > 80 
                                    ? c.Description.Substring(0, 80) + "..." 
                                    : c.Description,
                level = c.Level,
                category = c.Category?.Name ?? "General",
                reason = wrongCategories.Contains(c.CategoryId) 
                         ? "Sizin ���n x�susi: M?hz s?naqda s?hv etdiyiniz m�vzular? dolduracaq ?n uy?un kurs."
                         : level == "Intermediate"
                           ? "Haz?rk? bilikl?rinizi real layih?l?rd? t?tbiq etm?yiniz ���n �n?ririk."
                           : "Se�diyiniz sah? �zr? bacar?qlar? inki?af etdirm?y? k�m?k ed?c?k."
            }).ToList();

            // Generate Recommendations based on calculated Level
            string roadmap = level == "Beginner" 
                ? "T?m?l proqramla?d?rma anlay??lar?, Syntax, OOP prinsipl?ri �zr? baza yarad?n." 
                : level == "Intermediate" 
                ? "Data Structures, LINQ, Asinxron proqramla?d?rma v? Database arxitekturas?n? d?rind?n �yr?nin." 
                : "System Design, Microservices, Cloud Architecture (Azure/AWS) v? Design Patterns t?tbiq edin.";

            string strengths = level == "Advanced" ? "Analitik d�?�nc?, Kompleks M?ntiq" : level == "Intermediate" ? "Baza bilikl?ri, Alqoritmik yana?ma" : "�yr?nm?y? h?v?s, T?m?l m?ntiq";
            string weaknesses = level == "Advanced" ? "Sistem Arxitekturas? (B?lk?)" : level == "Intermediate" ? "Performance Optimization" : "D?rin OOP Prinsipl?ri, Design Patterns";

            if (wrongCategories.Any())
            {
                var weakCatNames = await _context.Categories.Where(c => wrongCategories.Contains(c.Id)).Select(c => c.Name).ToListAsync();
                if (weakCatNames.Any()) weaknesses += $" � Z?if M�vzular: {string.Join(", ", weakCatNames)}";
            }

            return Json(new
            {
                level = level,
                score = Math.Round(finalScore, 2),
                correctCount = correctCount,
                wrongCount = wrongCount,
                total = total,
                percentage = Math.Round(percentage * 100),
                xp = xp,
                roadmap = roadmap,
                strengths = strengths,
                weaknesses = weaknesses,
                recommendedCourses = recommendedCourses // Newly integrated data-driven recommendation
            });
        }
    }

    public class AssessmentSubmissionDto
    {
        public int? CategoryId { get; set; }
        public int? CourseId { get; set; }
        public int CheatAttempts { get; set; } 
        public List<AnswerDto> Answers { get; set; } = new List<AnswerDto>();
    }

    public class AnswerDto
    {
        public int QuestionId { get; set; }
        public int OptionId { get; set; }
        public int TimeTakenSeconds { get; set; } // Added for SpeedBonus
    }
}