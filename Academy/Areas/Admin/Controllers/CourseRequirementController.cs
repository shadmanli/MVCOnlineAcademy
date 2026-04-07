using Academy.Data;
using Academy.Services.Interfaces;
using Academy.ViewModels.CourseRequirement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Academy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CourseRequirementController : Controller
    {
        private readonly ICourseRequirementService _service;
        private readonly AppDbContext _context;

        public CourseRequirementController(ICourseRequirementService service, AppDbContext context)
        {
            _service = service;
            _context = context;
        }

      
        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAllAsync();
            return View(data);
        }

      
        public async Task<IActionResult> Create()
        {
            var model = new CourseRequirementCreateVM
            {
                Courses = await _context.Courses
                    .Select(x => new SelectListItem
                    {
                        Text = x.Title,
                        Value = x.Id.ToString()
                    }).ToListAsync()
            };

            return View(model);
        }

        
        [HttpPost]
        public async Task<IActionResult> Create(CourseRequirementCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                model.Courses = await _context.Courses
                    .Select(x => new SelectListItem
                    {
                        Text = x.Title,
                        Value = x.Id.ToString()
                    }).ToListAsync();

                return View(model);
            }

            await _service.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }

      
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }

      
        public async Task<IActionResult> Edit(int id)
        {
            var data = await _service.GetEditAsync(id);

            if (data == null) return NotFound();

            return View(data);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var data = await _service.GetByIdAsync(id);

            if (data == null) return NotFound();

            return View(data); 
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CourseRequirementEditVM model)
        {
            if (!ModelState.IsValid)
            {
                model.Courses = await _context.Courses
                    .Select(x => new SelectListItem
                    {
                        Text = x.Title,
                        Value = x.Id.ToString()
                    }).ToListAsync();

                return View(model);
            }

            await _service.EditAsync(model);
            return RedirectToAction(nameof(Index));
        }
    }
}
