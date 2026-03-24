using Academy.Data;
using Academy.Services.Interfaces;
using Academy.ViewModels.Article;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Academy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ArticleController(IArticleService articleService,
                                 AppDbContext context,
                                 IMapper mapper)
        {
            _articleService = articleService;
            _context = context;
            _mapper = mapper;
        }

        
        public async Task<IActionResult> Index()
        {
            var data = await _articleService.GetAllAsync();
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var topics = await _context.Topics.ToListAsync();

            var model = new ArticleCreateVM
            {
                Topics = topics.Select(x => new SelectListItem
                {
                    Text = x.Title,
                    Value = x.Id.ToString()
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ArticleCreateVM model)
        {
            await _articleService.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var data = await _articleService.GetByIdAsync(id);

            if (data == null) return NotFound();

            var res = _mapper.Map<ArticleDetailVM>(data);

            return View(res);
        }

     
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _articleService.DeleteAsync(id);
            return Ok();
        }
    }
}
