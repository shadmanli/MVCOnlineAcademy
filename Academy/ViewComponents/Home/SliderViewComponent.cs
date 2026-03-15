using Academy.Services.Interfaces;
using Academy.ViewModels.Slider;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Academy.ViewComponents.Slider
{
    public class SliderViewComponent:ViewComponent
    {
        private readonly ISliderService _sliderService;
        private readonly IMapper _mapper;
        public SliderViewComponent(ISliderService sliderService,IMapper mapper)
        {
            _mapper= mapper;
            _sliderService = sliderService;
        }
        public async Task< IViewComponentResult> InvokeAsync()
        {
            var slider = await _sliderService.GetAllAsync();
            var Sliders = _mapper.Map<IEnumerable<SliderVM>>(slider);
             
            return View(Sliders);  
        }
    }
}
