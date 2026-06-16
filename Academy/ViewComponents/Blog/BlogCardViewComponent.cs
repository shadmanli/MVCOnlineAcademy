using Academy.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Academy.ViewComponents.Blog
{
    public class BlogCardViewComponent : ViewComponent
    {
        private readonly IBlogService _blogService;
        private readonly IMapper _mapper;

        public BlogCardViewComponent(IBlogService blogService, IMapper mapper)
        {
            _blogService = blogService;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(int page = 1)
        {
            var all = (await _blogService.GetAllAsync()).ToList();
            const int pageSize = 6;
            int totalPages = (int)Math.Ceiling(all.Count / (double)pageSize);
            var paged = all.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages  = totalPages;

            return View(paged);
        }
    }
}
