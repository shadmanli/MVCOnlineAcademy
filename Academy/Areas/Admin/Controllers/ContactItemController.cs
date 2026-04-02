using Academy.Data;
using Academy.Services.Interfaces;
using Academy.ViewModels.ContactItem;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Academy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactItemController : Controller
    {
        private readonly IContactItemService _service;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ContactItemController(IContactItemService service,
                                     AppDbContext context,
                                     IMapper mapper)
        {
            _service = service;
            _context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _service.GetAllAsync());
        }

        public async Task<IActionResult> Create()
        {
            var sections = await _context.Contacts.ToListAsync();

            var model = new ContactItemCreateVM
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
        public async Task<IActionResult> Create(ContactItemCreateVM model)
        {
            await _service.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detail(int id)
        {
            var data = await _service.GetByIdAsync(id);
            return View(_mapper.Map<ContactItemDetailVM>(data));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }


        public async Task<IActionResult> Edit(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();

            var model = _mapper.Map<ContactItemEditVM>(item);

            var sections = await _context.Contacts.ToListAsync();
            model.Sections = sections.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ContactItemEditVM model)
        {
           

            await _service.UpdateAsync(model);
            return RedirectToAction(nameof(Index));
        }
    }
}
