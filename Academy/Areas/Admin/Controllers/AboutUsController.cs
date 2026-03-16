using Academy.Services.Interfaces;
using Academy.ViewModels.AboutUs;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AboutUsController : Controller
    {
        private readonly IAboutUsService _aboutUsService;
        private readonly IMapper _mapper;
        public AboutUsController(IAboutUsService aboutUsService,IMapper mapper)
        {
            _aboutUsService = aboutUsService;
            _mapper = mapper;
        }
        public async Task< IActionResult> Index()
        {
            var data = await _aboutUsService.GetAllAsync();
            var aboutUsVM =  _mapper.Map<IEnumerable<AboutUsVM>>(data);
            return View(aboutUsVM);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]  

        public async Task<IActionResult> Create(AboutUsCreateVM model)
        {
            await _aboutUsService.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }   

        public async Task<IActionResult> Detail(int id)
        {
            var data = await _aboutUsService.GetByIdAsync(id);
            if (data == null) return NotFound();
            var aboutusVM= _mapper.Map<AboutUsDetailVM>(data);
            return View(aboutusVM);
        }

        [HttpGet]

        public async Task<IActionResult> Delete(int id)
        {
            var data = await _aboutUsService.GetByIdAsync(id);
            if (data == null) return NotFound();
            await _aboutUsService.DeleteAsync(data);
            return Ok();
        }
    }
}
