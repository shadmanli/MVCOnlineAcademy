using Academy.Services.Interfaces;
using Academy.ViewModels.Blog;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Detail(int id)
        {
            var blog = await _blogService.GetByIdAsync(id);
            if (blog == null) return NotFound();

            var allBlogs = await _blogService.GetAllAsync();
            var related = allBlogs.Where(b => b.Id != id).Take(3).ToList();
            ViewBag.RelatedBlogs = related;

            return View(blog);
        }

        // AJAX: blog kartlarını page-ə görə gətir
        [HttpGet]
        public async Task<IActionResult> GetPage(int page = 1)
        {
            var all = (await _blogService.GetAllAsync()).ToList();
            const int pageSize = 6;
            int totalPages = (int)Math.Ceiling(all.Count / (double)pageSize);
            var paged = all.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages  = totalPages;

            return PartialView("_BlogPagePartial", paged);
        }
    }
}
