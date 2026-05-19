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
            return View("~/Areas/Admin/Views/AssessmentQuestion/Create.cshtml");
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(string DefaultText, int categoryId, List<string> optionTexts, int correctOptionIndex)
        {
            if (string.IsNullOrWhiteSpace(DefaultText) || optionTexts == null || optionTexts.Count == 0 || categoryId == 0)
                return BadRequest("Invalid question data or mapping");

            // Ignore empty strings in options
            var validOptions = optionTexts.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            if(validOptions.Count < 2) return BadRequest("At least 2 options are required.");

            var question = new AssessmentQuestion { Text = DefaultText, CategoryId = categoryId };

            for(int i = 0; i < validOptions.Count; i++)
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
            var question = await _context.AssessmentQuestions
                .Include(q => q.Options)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (question == null) return NotFound();

            return View("~/Areas/Admin/Views/AssessmentQuestion/Edit.cshtml", question);
        }

        [HttpPost("edit/{id}")]
        public async Task<IActionResult> Edit(int id, string DefaultText, int categoryId, List<string> optionTexts, int correctOptionIndex)
        {
            var question = await _context.AssessmentQuestions
                .Include(q => q.Options)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (question == null) return NotFound();

            if (string.IsNullOrWhiteSpace(DefaultText) || optionTexts == null || optionTexts.Count == 0 || categoryId == 0)
                return BadRequest("Invalid question data");

            var validOptions = optionTexts.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            if(validOptions.Count < 2) return BadRequest("At least 2 options are required.");

            question.Text = DefaultText;
            question.CategoryId = categoryId;

            // Remove old options and insert new
            _context.AssessmentOptions.RemoveRange(question.Options);

            var newOptions = new List<AssessmentOption>();
            for(int i = 0; i < validOptions.Count; i++)
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