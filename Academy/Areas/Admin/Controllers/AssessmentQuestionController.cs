using Academy.Data;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Academy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/assessment")]
    public class AssessmentQuestionController : Controller
    {
        private readonly AppDbContext _context;

        public AssessmentQuestionController(AppDbContext context)
        {
            _context = context;
        }

        [Route("")]
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            var questions = await _context.AssessmentQuestions
                .Include(q => q.Options)
                .ToListAsync();
            return View("~/Areas/Admin/Views/AssessmentQuestion/Index.cshtml", questions);
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Courses = await _context.Courses.ToListAsync();
            return View("~/Areas/Admin/Views/AssessmentQuestion/Create.cshtml");
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(string DefaultText, int categoryId, int? courseId, DifficultyLevel difficulty, int points, List<string> optionTexts, int correctOptionIndex)
        {
            if (string.IsNullOrWhiteSpace(DefaultText) || optionTexts == null || optionTexts.Count == 0 || categoryId == 0)
                return BadRequest("Invalid question data or mapping");

            var validOptions = optionTexts.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            if (validOptions.Count < 2) return BadRequest("At least 2 options are required.");

            // QuizId-ni courseId üzərindən tap, yoxdursa yarat
            int quizId = 0;
            if (courseId.HasValue)
            {
                var quiz = await _context.Quizzes
                    .FirstOrDefaultAsync(q => q.CourseId == courseId.Value);

                if (quiz == null)
                {
                    // Kurs üçün avtomatik Quiz yarat
                    var course = await _context.Courses.FindAsync(courseId.Value);
                    if (course == null)
                        return BadRequest($"CourseId={courseId} tapılmadı.");

                    quiz = new Quiz
                    {
                        Title = $"{course.Title} — Quiz",
                        CourseId = courseId.Value
                    };
                    _context.Quizzes.Add(quiz);
                    await _context.SaveChangesAsync();
                }

                quizId = quiz.Id;
            }
            else
            {
                // courseId yoxdursa — ilk mövcud quiz-i götür, yoxdursa avtomatik yarat
                var defaultQuiz = await _context.Quizzes.FirstOrDefaultAsync();
                if (defaultQuiz == null)
                {
                    var firstCourse = await _context.Courses.FirstOrDefaultAsync();
                    if (firstCourse == null)
                        return BadRequest("Əvvəlcə ən azı bir kurs yaradın.");

                    defaultQuiz = new Quiz
                    {
                        Title = $"{firstCourse.Title} — Quiz",
                        CourseId = firstCourse.Id
                    };
                    _context.Quizzes.Add(defaultQuiz);
                    await _context.SaveChangesAsync();
                }
                quizId = defaultQuiz.Id;
            }

            var question = new AssessmentQuestion
            {
                Text = DefaultText,
                CategoryId = categoryId,
                CourseId = courseId,
                QuizId = quizId,
                Difficulty = difficulty,
                Points = points
            };

            for (int i = 0; i < validOptions.Count; i++)
            {
                question.Options.Add(new AssessmentOption
                {
                    Text = validOptions[i],
                    IsCorrect = (i == correctOptionIndex)
                });
            }

            _context.AssessmentQuestions.Add(question);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Courses = await _context.Courses.ToListAsync();
            var question = await _context.AssessmentQuestions
                .Include(q => q.Options)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (question == null) return NotFound();

            return View("~/Areas/Admin/Views/AssessmentQuestion/Edit.cshtml", question);
        }

        [HttpPost("edit/{id}")]
        public async Task<IActionResult> Edit(int id, string DefaultText, int categoryId, int? courseId, DifficultyLevel difficulty, int points, List<string> optionTexts, int correctOptionIndex)
        {
            var question = await _context.AssessmentQuestions
                .Include(q => q.Options)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (question == null) return NotFound();

            if (string.IsNullOrWhiteSpace(DefaultText) || optionTexts == null || optionTexts.Count == 0 || categoryId == 0)
                return BadRequest("Invalid question data");

            var validOptions = optionTexts.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            if (validOptions.Count < 2) return BadRequest("At least 2 options are required.");

            // QuizId-ni yenilə (courseId dəyişibsə)
            if (courseId.HasValue && courseId != question.CourseId)
            {
                var quiz = await _context.Quizzes
                    .FirstOrDefaultAsync(q => q.CourseId == courseId.Value);

                if (quiz == null)
                {
                    var course = await _context.Courses.FindAsync(courseId.Value);
                    if (course != null)
                    {
                        quiz = new Quiz
                        {
                            Title = $"{course.Title} — Quiz",
                            CourseId = courseId.Value
                        };
                        _context.Quizzes.Add(quiz);
                        await _context.SaveChangesAsync();
                    }
                }

                if (quiz != null)
                    question.QuizId = quiz.Id;
            }

            question.Text = DefaultText;
            question.CategoryId = categoryId;
            question.CourseId = courseId;
            question.Difficulty = difficulty;
            question.Points = points;

            _context.AssessmentOptions.RemoveRange(question.Options);

            var newOptions = new List<AssessmentOption>();
            for (int i = 0; i < validOptions.Count; i++)
            {
                newOptions.Add(new AssessmentOption
                {
                    Text = validOptions[i],
                    IsCorrect = (i == correctOptionIndex),
                    AssessmentQuestionId = question.Id
                });
            }
            _context.AssessmentOptions.AddRange(newOptions);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var question = await _context.AssessmentQuestions.FindAsync(id);
            if (question != null)
            {
                _context.AssessmentQuestions.Remove(question);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

    }
}