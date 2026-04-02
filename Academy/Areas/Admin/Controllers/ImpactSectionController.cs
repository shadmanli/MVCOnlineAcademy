using Academy.Services.Interfaces;
using Academy.ViewModels.ImpactSection;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ImpactSectionController : Controller
    {
        private readonly IImpactSectionService _impactSectionService;
        private readonly IMapper _mapper;
        public ImpactSectionController(IImpactSectionService impactSectionService, IMapper mapper)
        {
            _impactSectionService = impactSectionService;
            _mapper = mapper;
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


        public async Task<IActionResult> Edit(int id)
        {
            var section = await _impactSectionService.GetByIdAsync(id);
            if (section == null) return NotFound();

            var model = _mapper.Map<ImpactSectionEditVM>(section);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ImpactSectionEditVM model)
        {
           

            await _impactSectionService.UpdateAsync(model);
            return RedirectToAction(nameof(Index));
        }
    }
}
