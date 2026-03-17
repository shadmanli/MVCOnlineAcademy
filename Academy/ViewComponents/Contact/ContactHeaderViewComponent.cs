using Academy.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Academy.ViewComponents.Contact
{
    public class ContactHeaderViewComponent:ViewComponent
    {
        private readonly IBannerService _bannerService;
        private readonly IMapper _mapper;
        public ContactHeaderViewComponent(IBannerService bannerService,IMapper mapper)
        {
            _bannerService = bannerService;
              _mapper = mapper;
        }
        public  async Task<IViewComponentResult> InvokeAsync()
        {
            var data = await _bannerService.GetAllAsync();

            return View(data);
        }
    }
}
