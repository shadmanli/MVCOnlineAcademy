using Academy.Data;
using Academy.Services;
using Academy.Services.Interfaces;
using Academy.ViewModels.Course;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Academy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SuperAdmin,Admin,Muellim")]
    public class CourseController : Controller
    {
        private readonly ICourseService _service;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CourseController(ICourseService service, AppDbContext context,IMapper mapper)
        {
                _mapper = mapper;
            _service = service;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _service.GetAllAsync());
        }

        public async Task<IActionResult> Create()
        {
            var model = new CourseCreateVM
            {
                Languages   = await _context.Languages
                    .Select(x => new SelectListItem(x.Name ?? "", x.Id.ToString()))
                    .ToListAsync(),
                Categories  = await _context.Categories
                    .Select(x => new SelectListItem(x.Name ?? "", x.Id.ToString()))
                    .ToListAsync(),
                Instructors = await _context.Instructors
                    .Where(x => x.FullName != null)
                    .Select(x => new SelectListItem(x.FullName, x.Id.ToString()))
                    .ToListAsync(),
                CourseNameItems = await _context.CourseNames
                    .Where(cn => cn.IsActive)
                    .OrderBy(cn => cn.Name)
                    .Select(cn => new SelectListItem(cn.Name, cn.Id.ToString()))
                    .ToListAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CourseCreateVM model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                await _service.CreateAsync(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                model.Languages   = await _context.Languages
                    .Select(x => new SelectListItem(x.Name ?? "", x.Id.ToString()))
                    .ToListAsync();
                model.Categories  = await _context.Categories
                    .Select(x => new SelectListItem(x.Name ?? "", x.Id.ToString()))
                    .ToListAsync();
                model.Instructors = await _context.Instructors
                    .Where(x => x.FullName != null)
                    .Select(x => new SelectListItem(x.FullName, x.Id.ToString()))
                    .ToListAsync();
                model.CourseNameItems = await _context.CourseNames
                    .Where(cn => cn.IsActive)
                    .OrderBy(cn => cn.Name)
                    .Select(cn => new SelectListItem(cn.Name, cn.Id.ToString()))
                    .ToListAsync();
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        // AJAX: kateqoriyaya görə müəllimləri gətir
        [HttpGet]
        public async Task<IActionResult> GetInstructorsByCategory(int categoryId)
        {
            // Həmin kateqoriyada kursu olan müəllimlər
            var instructorIds = await _context.Courses
                .Where(c => c.CategoryId == categoryId)
                .Select(c => c.InstructorId)
                .Distinct()
                .ToListAsync();

            List<object> instructors;

            if (instructorIds.Any())
            {
                // Kateqoriyaya aid müəllimlər
                instructors = await _context.Instructors
                    .Where(i => instructorIds.Contains(i.Id))
                    .Select(i => new { id = i.Id, name = i.FullName })
                    .ToListAsync<object>();
            }
            else
            {
                // Bu kateqoriyada hələ kurs yoxdursa — bütün müəllimləri göstər
                instructors = await _context.Instructors
                    .Where(i => i.FullName != null)
                    .Select(i => new { id = i.Id, name = i.FullName })
                    .ToListAsync<object>();
            }

            return Json(instructors);
        }


        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }

        public async Task<IActionResult> Edit(int id)
        {
            var data = await _context.Courses
                .Include(x => x.Videos)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (data == null) return NotFound();

            var model = _mapper.Map<CourseEditVM>(data);
            model.ExistingVideos = data.Videos;

            // 🔥 Dropdownları doldur + selected ver
            model.Languages = await _context.Languages
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                    Selected = x.Id == model.LanguageId
                }).ToListAsync();

            model.Categories = await _context.Categories
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                    Selected = x.Id == model.CategoryId
                }).ToListAsync();

            model.Instructors = await _context.Instructors
                .Select(x => new SelectListItem
                {
                    Text = x.FullName,
                    Value = x.Id.ToString(),
                    Selected = x.Id == model.InstructorId
                }).ToListAsync();

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(CourseEditVM model)
        {
            // 🔥 Update et
            await _service.UpdateAsync(model);

            // 🔥 Əgər səhifəyə qayıtmaq istəsən (məsələn error üçün)
            model.Languages = await _context.Languages
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToListAsync();

            model.Categories = await _context.Categories
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToListAsync();

            model.Instructors = await _context.Instructors
                .Select(x => new SelectListItem
                {
                    Text = x.FullName,
                    Value = x.Id.ToString()
                }).ToListAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
