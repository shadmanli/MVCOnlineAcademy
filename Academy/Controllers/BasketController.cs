using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Academy.Services.Interfaces;
using Academy.Data;
using Academy.ViewModels.Basket;

namespace Academy.Controllers
{
    public class BasketController : Controller
    {
        private readonly IBasketService _basketService;
        private readonly AppDbContext _context;

        public BasketController(IBasketService basketService, AppDbContext context)
        {
            _basketService = basketService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Sessiondan alinan courseId-leri yoxlayir
            var basketIds = _basketService.GetBasketCourseIds();
            var basketVMs = new List<BasketVM>();

            if (basketIds != null && basketIds.Any())
            {
                var courses = await _context.Courses
                    .Where(c => basketIds.Contains(c.Id))
                    .ToListAsync();

                foreach (var course in courses)
                {
                    basketVMs.Add(new BasketVM
                    {
                        CourseId = course.Id,
                        Title = course.Title,
                        ImageUrl = course.ImageUrl,
                        Price = course.Price
                    });
                }
            }

            return View(basketVMs);
        }

        [HttpPost]
        public IActionResult Add(int courseId)
        {
            var added = _basketService.AddToBasket(courseId, User.Identity.IsAuthenticated ? User.Identity.Name : null); 
            if (!added)
            {
                TempData["Warning"] = "Bu kursu art?q ?lav? etmisiniz.";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Remove(int courseId)
        {
            _basketService.RemoveFromBasket(courseId);
            return RedirectToAction("Index");
        }
    }
}