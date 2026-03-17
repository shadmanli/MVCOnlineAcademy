using Academy.Services.Interfaces;
using Academy.ViewModels.Banner;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.CodeDom;

namespace Academy.ViewComponents.About
{
    public class AboutViewComponent:ViewComponent
    {
        private readonly IBannerService _bannerService;
        private readonly IMapper _mapper;
        public AboutViewComponent(IBannerService bannerService,IMapper mapper)
        {
            _bannerService = bannerService;
            _mapper = mapper;
        }
        public async Task< IViewComponentResult> InvokeAsync()
        {
                var data = await _bannerService.GetAllAsync();
                var vm = _mapper.Map<IEnumerable<BannerVM>>(data);
            
            return View(vm);

        }
    }
}
