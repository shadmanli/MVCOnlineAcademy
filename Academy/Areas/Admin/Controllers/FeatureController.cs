using Academy.Services.Interfaces;
using Academy.ViewModels.Feature;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FeatureController : Controller
    {
        private readonly IFeatureService _featureService;
        private readonly IMapper _mapper;
        public FeatureController(IFeatureService featureService, IMapper mapper)
        {
            _featureService = featureService;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var data = await _featureService.GetAllAsync();
            return View(data);

        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(FeatureCreateVM model)
        {

            await _featureService.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var data = await _featureService.GetByIdAsync(id);
            if (data == null) return NotFound();
            var featureVM = _mapper.Map<FeatureDetailVM>(data);
            return View(featureVM);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _featureService.GetByIdAsync(id);
            if(data == null) return NotFound();
            await _featureService.DeleteAsync(data);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _featureService.GetByIdForEditAsync(id);
            if (model == null) return NotFound();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(FeatureEditVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _featureService.UpdateAsync(model);
            return RedirectToAction(nameof(Index));
        }
    }
}
