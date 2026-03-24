using Academy.Services.Interfaces;
using Academy.ViewModels.ImpactSection;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ImpactSectionController : Controller
    {
        private readonly IImpactSectionService _impactSectionService;
        public ImpactSectionController(IImpactSectionService impactSectionService)
        {
            _impactSectionService = impactSectionService;
        }
        public async Task<IActionResult> Index()
        {
            var data = await _impactSectionService.GetAllAsync();
            return View(data);
        }
        [HttpGet]

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ImpactSectionCreateVM model)
        {

            await _impactSectionService.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]

        public async Task<IActionResult> Detail(int id)
        {
            var data = await _impactSectionService.GetByIdAsync(id);
            return View(data);
        }



        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _impactSectionService.DeleteAsync(id);
            return Ok();
        }
    }
}
