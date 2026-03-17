using Academy.Services.Interfaces;
using Academy.ViewModels.Blog;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly IMapper _mapper;
        public BlogController(IBlogService blogService, IMapper mapper)
        {
            _blogService = blogService;
            _mapper = mapper;

        }
        public async Task<IActionResult> Index()
        {
            var model = await _blogService.GetAllAsync();
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(BlogCreateVM model)
        {

            await _blogService.CreateAsycn(model);
            return RedirectToAction(nameof(Index));

        }
    }
}
