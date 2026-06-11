
using Academy.Models;
using Academy.Services.Interfaces;
using Academy.ViewModels.Blog;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Areas.Admin.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin,Muellim")]
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
            if (!ModelState.IsValid) return View(model);

            try
            {
                await _blogService.CreateAsycn(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var model = await _blogService.GetByIdAsync(id);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _blogService.DeleteAsync(id); 
            return Ok();
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var data = await _blogService.GetByIdAsync(id);
            if (data == null) return NotFound();

            var model = _mapper.Map<BlogEditVM>(data);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, BlogEditVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _blogService.UpdateAsync(id, model);
            return RedirectToAction(nameof(Index));
        }
    }
}