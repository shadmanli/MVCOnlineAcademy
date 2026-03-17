using Academy.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Academy.ViewComponents.Blog
{
    public class BlogCardViewComponent:ViewComponent
    {
        private readonly IBlogService _blogService;
        private readonly IMapper _mapper;
        public BlogCardViewComponent(IBlogService blogService,IMapper mapper)
        {
            _blogService = blogService;
            _mapper = mapper;   
        }
        public async Task< IViewComponentResult> InvokeAsync()
        {
            var data = await _blogService.GetAllAsync();
            
            return View(data);
        }
    }
}
