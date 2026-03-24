using Academy.Services.Interfaces;
using Academy.ViewModels.About;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Academy.ViewComponents.About
{
    public class AboutAcademyViewComponent:ViewComponent
    {
        private readonly IAboutService _aboutService;
        private readonly IMapper _mapper;
        public AboutAcademyViewComponent(IAboutService aboutService,IMapper mapper)
        {
            _aboutService = aboutService;
            _mapper = mapper;
        }
        public async Task< IViewComponentResult> InvokeAsync()
        { 
            var data = await _aboutService.GetAllAsync();
            var res = data.First();
            var aboutVM = _mapper.Map<AboutVM>(res);
            return View(aboutVM);
        }
    }
}
