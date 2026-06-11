using Academy.Services.Interfaces;
using Academy.ViewModels.Slider;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Academy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SuperAdmin,Admin,Muellim")]
    public class SliderController : Controller
    {


        private readonly ISliderService _sliderService;
        private readonly IMapper _mapper;
        public SliderController(ISliderService sliderService, IMapper mapper)
        {
            _mapper = mapper;
            _sliderService = sliderService;
        }

        public async Task<IActionResult> Index()
        {
            var slider = await _sliderService.GetAllAsync();
            var SliderVM = _mapper.Map<IEnumerable<SliderVM>>(slider);
            return View(SliderVM);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(SliderCreateVM model)
        {
            await _sliderService.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Detail(int id)
        {
            var slider = await _sliderService.GetByIdAsync(id);
            if (slider == null) return NotFound();
            var SliderVM = _mapper.Map<SliderDetailVM>(slider);
            return View(SliderVM);

        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var slider = await _sliderService.GetByIdAsync(id);
            if (slider == null) return NotFound();
            await _sliderService.DeleteAsync(slider);
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]

        public async Task<IActionResult> Edit(int id)
        {
            var slider = await _sliderService.GetByIdAsync(id);
            var sliderEditVM = new SliderEditVM
            {
                Id = slider.Id,
                Title = slider.Title,
                Description = slider.Description,
                Image = slider.Image,
            };
            return View(sliderEditVM);
        }

        [HttpPost]

        public async Task<IActionResult> Edit(SliderEditVM model)
        {
            await _sliderService.EditAsync(model);
            return RedirectToAction(nameof(Index));
        }
    }
}
