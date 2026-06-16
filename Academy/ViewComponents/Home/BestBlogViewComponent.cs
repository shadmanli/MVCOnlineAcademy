using Academy.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Academy.ViewComponents.Home
{
    public class BestBlogViewComponent : ViewComponent
    {
        private readonly IBlogService _blogService;
        private readonly IMapper _mapper;

        public BestBlogViewComponent(IBlogService blogService, IMapper mapper)
        {
            _blogService = blogService;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Son 3 blogu gətir
            var data = (await _blogService.GetAllAsync())
                .OrderByDescending(x => x.Id)
                .Take(3)
                .ToList();

            return View(data);
        }
    }
}
