using Academy.Services.Interfaces;
using Academy.ViewModels.About;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Areas.Admin.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin,Muellim")]
    [Area("Admin")]
    public class AboutController : Controller
    {
        private readonly IAboutService _aboutService;
        private readonly IMapper _mapper;
        public AboutController(IAboutService aboutService, IMapper mapper)
        {
            _aboutService = aboutService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _aboutService.GetAllAsync();
            return View(data);

        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(AboutCreateVM about)
        {

            await _aboutService.CreateAsync(about);
            return RedirectToAction(nameof(Index));

        }


        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var data = await _aboutService.GetByIdAsync(id); 

            if (data == null)
            {
                return NotFound();
            }

            return View(data); 
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _aboutService.DeleteAsync(id);
            return Ok();
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var data = await _aboutService.GetEntityByIdAsync(id);

            if (data == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<AboutEditVM>(data); 

            return View(model);
        }


        [HttpPost]
     
        public async Task<IActionResult> Edit(int id, AboutEditVM model)
        {
            if (id != model.Id)
                return BadRequest();

            await _aboutService.EditAsync(model);

            return RedirectToAction(nameof(Index));
        }
    }
}