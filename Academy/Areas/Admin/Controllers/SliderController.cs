using Academy.Services.Interfaces;
using Academy.ViewModels.Slider;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Academy.Areas.Admin.Controllers
{
    [Area("Admin")]
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
    }
}
