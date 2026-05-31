using Microsoft.AspNetCore.Mvc;
using Academy.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Academy.Models;
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

        public IActionResult Index(int? categoryId, int? courseId)
        {
            ViewBag.CategoryId = categoryId;
            ViewBag.CourseId = courseId;
            return View();
        }

        // ─────────────────────────────────────────────────────────────
        // GET /Assessment/GetRecommendedLessons?courseId=5&level=Beginner
        // Həmin kursun videolarını istifadəçinin səviyyəsinə görə qaytarır
        // ─────────────────────────────────────────────────────────────
        [HttpGet]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> GetRecommendedLessons(int courseId, string level)
        {
            if (courseId <= 0)
                return BadRequest("CourseId lazımdır.");

            // Level-i VideoLevel enum-a çevir
            VideoLevel targetLevel = level switch {
                "Advanced"     => VideoLevel.Advanced,
                "Intermediate" => VideoLevel.Intermediate,
                _              => VideoLevel.Beginner
            };

            // Həmin kursun videolarını tap
            var course = await _context.Courses
                .Include(c => c.Videos)
                .Include(c => c.Lessons)
                .FirstOrDefaultAsync(c => c.Id == courseId);

            if (course == null)
                return NotFound("Kurs tapılmadı.");

            // İstifadəçinin səviyyəsinə uyğun videolar
            // Beginner → Beginner videolar
            // Intermediate → Beginner + Intermediate videolar
            // Advanced → hamısı
            var filteredVideos = course.Videos
                .Where(v => level == "Advanced"
                    ? true
                    : level == "Intermediate"
                        ? v.Level == VideoLevel.Beginner || v.Level == VideoLevel.Intermediate
                        : v.Level == VideoLevel.Beginner)
                .OrderBy(v => v.Level)
                .ThenBy(v => v.Id)
                .Select(v => new {
                    id    = v.Id,
                    title = v.Title,
                    url   = v.Url,
                    level = v.Level.ToString()
                })
                .ToList();

            // Dərslər (Lessons) — kursun bütün dərsləri
            var lessons = course.Lessons
                .OrderBy(l => l.Id)
                .Select(l => new {
                    id    = l.Id,
                    title = l.Title
                })
                .ToList();

            return Json(new {
                courseId    = course.Id,
                courseTitle = course.Title,
                userLevel   = level,
                videos      = filteredVideos,
                lessons     = lessons,
                totalVideos = filteredVideos.Count,
                message     = filteredVideos.Any()
                    ? $"{level} səviyyəsi üçün {filteredVideos.Count} video tövsiyə edilir."
                    : "Bu kursda sizin səviyyənizə uyğun video tapılmadı."
            });
        }
        [HttpGet]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> GetQuestions(int courseId)
        {
            var rng = new Random();

            // 1. Kursun quiz-ini tap
            if (courseId > 0)
            {
                var quiz = await _context.Quizzes
                    .Include(q => q.Questions)
                    .ThenInclude(q => q.Options)
                    .FirstOrDefaultAsync(q => q.CourseId == courseId);

                if (quiz != null && quiz.Questions.Any(q => q.Options.Any()))
                {
                    var list = quiz.Questions
                        .Where(q => q.Options.Any())
                        .OrderBy(_ => rng.Next())
                        .Take(15)
                        .Select(q => new {
                            id         = q.Id,
                            text       = q.Text,
                            difficulty = (int)q.Difficulty,
                            points     = q.Points,
                            options    = q.Options
                                .OrderBy(_ => rng.Next())
                                .Select(o => new { id = o.Id, text = o.Text })
                                .ToList()
                        });
                    return Json(list);
                }
            }

            // 2. Kurs quiz-i yoxdursa — bütün sualları qaytart
            var all = await _context.AssessmentQuestions
                .Include(q => q.Options)
                .Where(q => q.Options.Any())
                .ToListAsync();

            if (!all.Any())
                return NotFound("Hələ sual əlavə edilməyib.");

            var result = all
                .OrderBy(_ => rng.Next()).Take(15)
                .Select(q => new {
                    id         = q.Id,
                    text       = q.Text,
                    difficulty = (int)q.Difficulty,
                    points     = q.Points,
                    options    = q.Options
                        .OrderBy(_ => rng.Next())
                        .Select(o => new { id = o.Id, text = o.Text })
                        .ToList()
                });

            return Json(result);
        }

        // ─────────────────────────────────────────────────────────────
        // POST /Assessment/SubmitAssessment
        // ─────────────────────────────────────────────────────────────
        [HttpPost]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> SubmitAssessment([FromBody] AssessmentSubmissionDto submission)
        {
            if (submission?.Answers == null || !submission.Answers.Any())
                return BadRequest("Cavablar boş ola bilməz.");

            // ── 1. Quiz tap ──────────────────────────────────────────
            Quiz? quiz = null;
            if (submission.CourseId.HasValue && submission.CourseId > 0)
                quiz = await _context.Quizzes
                    .FirstOrDefaultAsync(q => q.CourseId == submission.CourseId.Value);

            if (quiz == null)
            {
                var firstQId = submission.Answers.FirstOrDefault()?.QuestionId ?? 0;
                if (firstQId > 0)
                {
                    var fq = await _context.AssessmentQuestions
                        .Include(q => q.Quiz)
                        .FirstOrDefaultAsync(q => q.Id == firstQId);
                    quiz = fq?.Quiz;
                }
            }

            if (quiz == null)
                quiz = await _context.Quizzes.FirstOrDefaultAsync();

            if (quiz == null)
                return BadRequest("Quiz tapılmadı. Əvvəlcə admin paneldən quiz yaradın.");

            // ── 2. Bütün sualları bir dəfəyə yüklə ──────────────────
            var questionIds = submission.Answers
                .Select(a => a.QuestionId).Distinct().ToList();

            var questions = await _context.AssessmentQuestions
                .Include(q => q.Options)
                .Where(q => questionIds.Contains(q.Id))
                .ToDictionaryAsync(q => q.Id);

            // ── 3. Cavabları qiymətləndir ────────────────────────────
            int correctCount  = 0;
            int wrongCount    = 0;
            int total         = submission.Answers.Count;
            int currentCombo  = 0;
            int maxCombo      = 0;
            var wrongQIds     = new List<int>();

            foreach (var answer in submission.Answers)
            {
                if (!questions.TryGetValue(answer.QuestionId, out var q)) continue;

                var opt = q.Options.FirstOrDefault(o => o.Id == answer.OptionId);
                bool correct = opt?.IsCorrect ?? false;

                if (correct)
                {
                    correctCount++;
                    currentCombo++;
                    if (currentCombo > maxCombo) maxCombo = currentCombo;
                }
                else
                {
                    wrongCount++;
                    currentCombo = 0;
                    wrongQIds.Add(answer.QuestionId);
                }
            }

            // ── 4. Dəqiqlik: correctCount / total * 100 ──────────────
            double percentage = total > 0
                ? Math.Round((double)correctCount / total * 100, 1)
                : 0;

            // ── 5. Level məntiqi ─────────────────────────────────────
            // 0-40%  → Beginner
            // 41-70% → Intermediate
            // 71-100%→ Advanced
            string level = percentage >= 71 ? "Advanced"
                         : percentage >= 41 ? "Intermediate"
                         : "Beginner";

            // ── 6. XP: faiz əsaslı, max 500 XP ──────────────────────
            // Kombo bonusu: hər 3+ ardıcıl düzgün cavab +10 XP
            int baseXp  = (int)Math.Round(percentage * 5);
            int comboXp = maxCombo >= 3 ? (maxCombo / 3) * 10 : 0;
            int xp      = Math.Min(baseXp + comboXp, 500);

            // ── 7. CategoryId tap ────────────────────────────────────
            int categoryId = submission.CategoryId ?? 0;
            if (categoryId == 0 && questions.Any())
                categoryId = questions.Values.First().CategoryId;
            if (categoryId == 0 && submission.CourseId.HasValue)
            {
                var course = await _context.Courses.FindAsync(submission.CourseId.Value);
                categoryId = course?.CategoryId ?? 0;
            }
            if (categoryId == 0)
            {
                var firstCat = await _context.Categories.FirstOrDefaultAsync();
                categoryId = firstCat?.Id ?? 1;
            }

            // ── 8. Nəticəni DB-yə yaz ───────────────────────────────
            var appUser = await _userManager.GetUserAsync(User);
            if (appUser != null)
            {
                var newResult = new UserAssessmentResult
                {
                    AppUserId      = appUser.Id,
                    CategoryId     = categoryId,
                    CourseId       = submission.CourseId,
                    QuizId         = quiz.Id,
                    Score          = correctCount,
                    TotalQuestions = total,
                    Percentage     = percentage,
                    Level          = level,
                    XP             = xp,
                    CreatedAt      = DateTime.Now
                };
                _context.UserAssessmentResults.Add(newResult);
                await _context.SaveChangesAsync();
            }

            // ── 9. Zəif kateqoriyalar ────────────────────────────────
            var weakCatIds = questions.Values
                .Where(q => wrongQIds.Contains(q.Id))
                .Select(q => q.CategoryId)
                .Distinct()
                .ToList();

            // ── 10. Tövsiyə sistemi ──────────────────────────────────
            // Məntiqi:
            // a) İstifadəçinin zəif olduğu kateqoriyalardakı kurslar (ən yüksək prioritet)
            // b) İstifadəçinin level-inə uyğun kurslar
            // c) Hazırda baxdığı kursdan fərqli kurslar
            var allCourses = await _context.Courses
                .Include(c => c.Category)
                .Where(c => c.IsActive && !c.IsDeleted
                         && c.Id != (submission.CourseId ?? 0))
                .ToListAsync();

            var ranked = allCourses
                .Select(c => {
                    int score = 0;

                    // Zəif kateqoriyaya uyğundursa +30
                    if (weakCatIds.Contains(c.CategoryId)) score += 30;

                    // Level uyğunluğu
                    if (c.Level == level) score += 20;

                    // Beginner üçün Beginner kurslar əlavə boost
                    if (level == "Beginner" && c.Level == "Beginner") score += 10;

                    // Intermediate üçün həm Beginner həm Intermediate
                    if (level == "Intermediate" && c.Level == "Intermediate") score += 5;

                    // Advanced üçün Advanced kurslar
                    if (level == "Advanced" && c.Level == "Advanced") score += 10;

                    return new { Course = c, Score = score };
                })
                .Where(x => x.Score > 0)
                .OrderByDescending(x => x.Score)
                .Take(4)
                .ToList();

            // Əgər heç uyğun kurs yoxdursa — ən azı level-ə görə göstər
            if (!ranked.Any())
            {
                ranked = allCourses
                    .Select(c => new { Course = c, Score = c.Level == level ? 1 : 0 })
                    .Where(x => x.Score > 0)
                    .Take(4)
                    .ToList();
            }

            var weakCatNames = await _context.Categories
                .Where(c => weakCatIds.Contains(c.Id))
                .Select(c => c.Name)
                .ToListAsync();

            var recommendedCourses = ranked.Select(x => new {
                title            = x.Course.Title,
                imageUrl         = x.Course.ImageUrl,
                url              = Url.Action("Index", "CourseDetail", new { id = x.Course.Id }),
                shortDescription = x.Course.Description?.Length > 100
                    ? x.Course.Description.Substring(0, 100) + "..."
                    : x.Course.Description,
                level    = x.Course.Level,
                category = x.Course.Category?.Name ?? "Ümumi",
                reason   = weakCatIds.Contains(x.Course.CategoryId)
                    ? $"Sınaqda zəif olduğunuz \"{x.Course.Category?.Name}\" mövzusunu gücləndirir."
                    : x.Course.Level == level
                        ? $"Hazırkı {level} səviyyənizə tam uyğun kurs."
                        : "Biliklərinizi genişləndirmək üçün tövsiyə edilir."
            }).ToList();

            // ── 11. Yol xəritəsi ─────────────────────────────────────
            string roadmap = level switch {
                "Advanced"     => "System Design, Microservices, Cloud Architecture (Azure/AWS) və Design Patterns tətbiq edin.",
                "Intermediate" => "Data Structures, LINQ, Asinxron proqramlaşdırma və Database arxitekturasını dərindən öyrənin.",
                _              => "Təməl proqramlaşdırma anlayışları, Syntax, OOP prinsipləri üzrə baza yaradın."
            };

            string strengths = level switch {
                "Advanced"     => "Analitik düşüncə, Kompleks məntiq, Alqoritmik yanaşma",
                "Intermediate" => "Baza bilikləri, Alqoritmik yanaşma, Problem həll etmə",
                _              => "Öyrənməyə həvəs, Təməl məntiq, Yeni başlayanlar üçün güclü baza"
            };

            string weaknesses = level switch {
                "Advanced"     => "Sistem Arxitekturası, Distributed Systems",
                "Intermediate" => "Performance Optimization, Advanced Design Patterns",
                _              => "Dərin OOP Prinsipləri, Design Patterns, Alqoritmlər"
            };

            if (weakCatNames.Any())
                weaknesses += $" — Zəif mövzular: {string.Join(", ", weakCatNames)}";

            return Json(new {
                level,
                score          = correctCount,
                correctCount,
                wrongCount,
                total,
                percentage,
                xp,
                maxCombo,
                roadmap,
                strengths,
                weaknesses,
                recommendedCourses
            });
        }
    }

    public class AssessmentSubmissionDto
    {
        public int? CategoryId    { get; set; }
        public int? CourseId      { get; set; }
        public int  CheatAttempts { get; set; }
        public List<AnswerDto> Answers { get; set; } = new();
    }

    public class AnswerDto
    {
        public int QuestionId       { get; set; }
        public int OptionId         { get; set; }
        public int TimeTakenSeconds { get; set; }
    }
}
