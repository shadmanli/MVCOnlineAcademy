using Academy.Services.Interfaces;
using Academy.ViewModels.AboutUs;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Academy.ViewComponents.Advance
{
    public class AdvanceViewComponent : ViewComponent
    {
        private readonly IAboutUsService _aboutUsService;
        private readonly IMapper _mapper;

        public AdvanceViewComponent(IAboutUsService aboutUsService,IMapper mapper)
        {
            _mapper = mapper;
            _aboutUsService = aboutUsService;
        }
        public async Task< IViewComponentResult> InvokeAsync()
        {
            var about =  await _aboutUsService.GetAllAsync();
             var abouts = about.FirstOrDefault();
            var aboutVM = _mapper.Map<AboutUsVM>(abouts);
            return View(aboutVM);
        }
    }
}
