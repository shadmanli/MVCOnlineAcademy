using Academy.Services.Interfaces;
using Academy.ViewModels.Instructor;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SuperAdmin,Admin,Muellim")]
    public class InstructorController : Controller
    {
        private readonly IInstructorService _service;

        public InstructorController(IInstructorService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _service.GetAllAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(InstructorCreateVM model)
        {
            if (!ModelState.IsValid) return View(model);

            await _service.CreateAsync(model);
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

            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(InstructorEditVM model)
        {
            await _service.EditAsync(model);
            return RedirectToAction(nameof(Index));
        }
    }
}
