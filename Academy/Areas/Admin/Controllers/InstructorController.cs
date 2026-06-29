using Academy.Data;
using Academy.Models;
using Academy.Services.Interfaces;
using Academy.ViewModels.Instructor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Academy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin,Muellim")]
    public class InstructorController : Controller
    {
        private readonly IInstructorService _service;
        private readonly AppDbContext _context;

        public InstructorController(IInstructorService service, AppDbContext context)
        {
            _service = service;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _service.GetAllAsync());
        }

        public async Task<IActionResult> Create()
        {
            var model = new InstructorCreateVM
            {
                AllCategories = await _context.Categories
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                    .ToListAsync()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InstructorCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                model.AllCategories = await _context.Categories
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                    .ToListAsync();
                return View(model);
            }

            await _service.CreateAsync(model);

            // Kateqoriya əlaqəsini əlavə et
            if (model.CategoryIds.Any())
            {
                var instructor = await _context.Instructors
                    .OrderByDescending(i => i.Id)
                    .FirstOrDefaultAsync();
                if (instructor != null)
                {
                    foreach (var catId in model.CategoryIds)
                    {
                        _context.InstructorCategories.Add(new InstructorCategory
                        {
                            InstructorId = instructor.Id,
                            CategoryId   = catId
                        });
                    }
                    await _context.SaveChangesAsync();
                }
            }

            TempData["Success"] = $"\"{model.FullName}\" uğurla əlavə edildi.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detail(int id)
        {
            return View(await _service.GetByIdAsync(id));
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var data = await _service.GetEditByIdAsync(id);
            if (data == null) return NotFound();

            // Mövcud kateqoriyaları yüklə
            var existingCatIds = await _context.InstructorCategories
                .Where(ic => ic.InstructorId == id)
                .Select(ic => ic.CategoryId)
                .ToListAsync();

            data.CategoryIds = existingCatIds;
            data.AllCategories = await _context.Categories
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                .ToListAsync();

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(InstructorEditVM model)
        {
            if (!ModelState.IsValid)
            {
                model.AllCategories = await _context.Categories
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                    .ToListAsync();
                return View(model);
            }

            await _service.EditAsync(model);

            // Kateqoriyaları yenilə
            var existing = await _context.InstructorCategories
                .Where(ic => ic.InstructorId == model.Id)
                .ToListAsync();
            _context.InstructorCategories.RemoveRange(existing);

            if (model.CategoryIds.Any())
            {
                foreach (var catId in model.CategoryIds)
                {
                    _context.InstructorCategories.Add(new InstructorCategory
                    {
                        InstructorId = model.Id,
                        CategoryId   = catId
                    });
                }
            }
            await _context.SaveChangesAsync();

            TempData["Success"] = $"\"{model.FullName}\" uğurla yeniləndi.";
            return RedirectToAction(nameof(Index));
        }

        // AJAX: kateqoriyaya görə müəllimləri gətir
        [HttpGet]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            var instructors = await _context.InstructorCategories
                .Where(ic => ic.CategoryId == categoryId)
                .Select(ic => new { id = ic.Instructor.Id, name = ic.Instructor.FullName })
                .ToListAsync();

            // Bu kateqoriyada InstructorCategory yoxdursa, kurslara görə yoxla
            if (!instructors.Any())
            {
                var fromCourses = await _context.Courses
                    .Where(c => c.CategoryId == categoryId && c.InstructorId.HasValue)
                    .Select(c => new { id = c.Instructor!.Id, name = c.Instructor.FullName })
                    .Distinct()
                    .ToListAsync();
                instructors = fromCourses;
            }

            // Hər ikisi boşdursa bütün müəllimləri göstər
            if (!instructors.Any())
            {
                instructors = await _context.Instructors
                    .Where(i => i.FullName != null)
                    .Select(i => new { id = i.Id, name = i.FullName })
                    .ToListAsync();
            }

            return Json(instructors);
        }
    }
}
