using Academy.Data;
using Academy.Services.Interfaces;
using Academy.ViewModels.ImpactItem;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Academy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SuperAdmin,Admin,Muellim")]
    public class ImpactItemController : Controller
    {
        private readonly IImpactItemService _impactItemService;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public ImpactItemController(IImpactItemService impactItemService, IMapper mapper, AppDbContext context)
        {
            _impactItemService = impactItemService;
            _mapper = mapper;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _impactItemService.GetAllAsync();
            return View(data);
        }

  
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var sections = await _context.ImpactSections.ToListAsync();

            var model = new ImpactItemCreateVM
            {
                Sections = sections.Select(x => new SelectListItem
                {
                    Text = x.Title,
                    Value = x.Id.ToString()
                }).ToList()
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(ImpactItemCreateVM model)
        {
            await _impactItemService.CreateAsync(model);

            return RedirectToAction(nameof(Index)); 
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var data = await _impactItemService.GetByIdAsync(id);
            if (data == null) return NotFound();
            var res = _mapper.Map<ImpactItemDetailVM>(data);
            return View(res);
        }

        [HttpGet]

        public async Task<IActionResult> DeleteAsync(int id)
        {
           await _impactItemService.DeleteAsync(id);
            return Ok();
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _impactItemService.GetByIdAsync(id);
            if (item == null) return NotFound();

            var model = _mapper.Map<ImpactItemEditVM>(item);

            var sections = await _context.ImpactSections.ToListAsync();

            model.Sections = sections.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

            return View(model);
        }

        [HttpPost]
     
        public async Task<IActionResult> Edit(ImpactItemEditVM model)
        {
            await _impactItemService.UpdateAsync(model);
            return RedirectToAction(nameof(Index));
        }
    }
}