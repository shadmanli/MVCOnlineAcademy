using Academy.Services.Interfaces;
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

            // Related blogs (all except current)
            var allBlogs = await _blogService.GetAllAsync();
            var related = allBlogs.Where(b => b.Id != id).Take(3).ToList();
            ViewBag.RelatedBlogs = related;

            return View(blog);
        }
    }
}
