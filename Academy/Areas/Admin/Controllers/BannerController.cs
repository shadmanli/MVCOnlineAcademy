using Academy.Services.Interfaces;
using Academy.ViewModels.Banner;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BannerController : Controller
    {
        private readonly IBannerService _bannerService;
        private readonly IMapper _mapper;
        public BannerController(IBannerService bannerService, IMapper mapper)
        {
            _mapper = mapper;
            _bannerService = bannerService;
        }
        public async Task<IActionResult> Index()
        {

            var data = await _bannerService.GetAllAsync();
            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(BannerCreateVM model)
        {
            await _bannerService.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]

        public async Task<IActionResult> Detail(int id)
        {
            var data = await _bannerService.GetByIdAsync(id);
            if (data == null) return NotFound();
            return View(data);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _bannerService.GetByIdAsync(id);
            if (data == null) return NotFound();
            await _bannerService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
