using Academy.Data;
using Academy.Services.Interfaces;
using Academy.ViewModels.CourseName;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Academy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin,Muellim")]
    public class CourseNameController : Controller
    {
        private readonly ICourseNameService _service;
        private readonly AppDbContext _context;

        public CourseNameController(ICourseNameService service, AppDbContext context)
        {
            _service = service;
            _context = context;
        }

        // GET /Admin/CourseName
        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAllAsync();
            return View(data);
        }

        // GET /Admin/CourseName/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.AllCategories = await _context.Categories
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                .ToListAsync();
            return View(new CourseNameCreateVM());
        }

        // POST /Admin/CourseName/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseNameCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.AllCategories = await _context.Categories
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                    .ToListAsync();
                return View(model);
            }

            await _service.CreateAsync(model);
            TempData["Success"] = $"\"{model.Name}\" uğurla əlavə edildi.";
            return RedirectToAction(nameof(Index));
        }

        // GET /Admin/CourseName/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();

            ViewBag.AllCategories = await _context.Categories
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                .ToListAsync();

            return View(new CourseNameEditVM
            {
                Id          = item.Id,
                Name        = item.Name,
                Description = item.Description,
                IsActive    = item.IsActive,
                CategoryIds = item.CategoryIds
            });
        }

        // POST /Admin/CourseName/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CourseNameEditVM model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.AllCategories = await _context.Categories
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                    .ToListAsync();
                return View(model);
            }

            await _service.UpdateAsync(id, model);
            TempData["Success"] = $"\"{model.Name}\" uğurla yeniləndi.";
            return RedirectToAction(nameof(Index));
        }

        // POST /Admin/CourseName/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            TempData["Success"] = "Kurs adı silindi.";
            return RedirectToAction(nameof(Index));
        }

        // AJAX: GET /Admin/CourseName/GetCategories?courseNameId=5
        [HttpGet]
        public async Task<IActionResult> GetCategories(int courseNameId)
        {
            var cats = await _service.GetCategoriesByNameIdAsync(courseNameId);
            return Json(cats);
        }

        // AJAX: GET /Admin/CourseName/GetAllNames
        [HttpGet]
        public async Task<IActionResult> GetAllNames()
        {
            var names = await _service.GetActiveNamesAsync();
            return Json(names.Select(n => new { id = n.Id, name = n.Name }));
        }
    }
}
