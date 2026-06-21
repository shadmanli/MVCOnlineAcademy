using Academy.Data;
using Academy.Models;
using Academy.ViewModels.Partner;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Academy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin,Muellim")]
    public class PartnerController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public PartnerController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var partners = await _context.Partners
                .OrderBy(p => p.Order)
                .ThenBy(p => p.Id)
                .ToListAsync();

            var vm = partners.Select(p => new PartnerVM
            {
                Id = p.Id,
                Name = p.Name,
                Image = p.Image,
                Order = p.Order,
                IsActive = p.IsActive
            }).ToList();

            return View(vm);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PartnerCreateVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var partner = new Partner
            {
                Name = model.Name,
                Order = model.Order,
                IsActive = model.IsActive
            };

            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "partners");
                Directory.CreateDirectory(uploadsFolder);
                var fileName = Guid.NewGuid() + Path.GetExtension(model.ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await model.ImageFile.CopyToAsync(stream);
                partner.Image = fileName;
            }

            _context.Partners.Add(partner);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Partner uğurla əlavə edildi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var partner = await _context.Partners.FindAsync(id);
            if (partner == null) return NotFound();

            var vm = new PartnerEditVM
            {
                Id = partner.Id,
                Name = partner.Name,
                Image = partner.Image,
                Order = partner.Order,
                IsActive = partner.IsActive
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PartnerEditVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var partner = await _context.Partners.FindAsync(model.Id);
            if (partner == null) return NotFound();

            partner.Name = model.Name;
            partner.Order = model.Order;
            partner.IsActive = model.IsActive;

            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                // Delete old image
                if (!string.IsNullOrEmpty(partner.Image))
                {
                    var oldPath = Path.Combine(_env.WebRootPath, "uploads", "partners", partner.Image);
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "partners");
                Directory.CreateDirectory(uploadsFolder);
                var fileName = Guid.NewGuid() + Path.GetExtension(model.ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await model.ImageFile.CopyToAsync(stream);
                partner.Image = fileName;
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Partner uğurla yeniləndi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var partner = await _context.Partners.FindAsync(id);
            if (partner == null) return NotFound();

            if (!string.IsNullOrEmpty(partner.Image))
            {
                var imgPath = Path.Combine(_env.WebRootPath, "uploads", "partners", partner.Image);
                if (System.IO.File.Exists(imgPath))
                    System.IO.File.Delete(imgPath);
            }

            _context.Partners.Remove(partner);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Partner silindi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var partner = await _context.Partners.FindAsync(id);
            if (partner == null) return NotFound();
            partner.IsActive = !partner.IsActive;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
