using Academy.Services.Interfaces;
using Academy.ViewModels.ContactSection;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactSectionController : Controller
    {
        private readonly IContactSectionService _service;
        private readonly IMapper _mapper;
        public ContactSectionController(IContactSectionService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _service.GetAllAsync());
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(ContactSectionCreateVM model)
        {
            await _service.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detail(int id)
        {
            return View(await _service.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }


        public async Task<IActionResult> Edit(int id)
        {
            var section = await _service.GetByIdAsync(id);
            if (section == null) return NotFound();

            var model = _mapper.Map<ContactSectionEditVM>(section);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ContactSectionEditVM model)
        {
            if (!ModelState.IsValid) return View(model);

            await _service.UpdateAsync(model);
            return RedirectToAction(nameof(Index));
        }
    }
}
