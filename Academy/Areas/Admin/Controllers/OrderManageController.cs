using Academy.Data;
using Academy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Academy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class OrderManageController : Controller
    {
        private readonly AppDbContext _context;

        public OrderManageController(AppDbContext context)
        {
            _context = context;
        }

        // Bütün sifarişlər
        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Course)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
            return View(orders);
        }

        // Manual Paid et
        [HttpPost]
        public async Task<IActionResult> MarkAsPaid(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();
            order.Status = OrderStatus.Paid;
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Sifariş #{order.OrderNumber} Paid olaraq işarələndi.";
            return RedirectToAction(nameof(Index));
        }

        // Manual Pending et
        [HttpPost]
        public async Task<IActionResult> MarkAsPending(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();
            order.Status = OrderStatus.Pending;
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Sifariş #{order.OrderNumber} Pending olaraq işarələndi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
