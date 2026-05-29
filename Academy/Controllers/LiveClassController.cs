using Academy.Data;
using Academy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Academy.Controllers
{
    public class LiveClassController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public LiveClassController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // T?l?b? v? Mü?llim üçün Canl? Otaq (UI)
        public async Task<IActionResult> Room(string id)
        {
            var liveClass = await _context.LiveClasses
                .Include(l => l.Course)
                .Include(l => l.Teacher)
                .FirstOrDefaultAsync(l => l.RoomId == id);

            bool isTeacher = false;
            if (liveClass == null) 
            {
                // UI yoxlamaq üçün mock m?lumat (?g?r ID uy?un g?lirs? v? ya yoxdursa)
                ViewBag.Title = "Demo Canl? D?rs";
                ViewBag.Instructor = "Proqramla?d?rma Mü?llimi";
                ViewBag.RoomId = id ?? "demo-room";
            }
            else
            {
                ViewBag.Title = liveClass.Title;
                ViewBag.Instructor = liveClass.Teacher != null ? (liveClass.Teacher.FullName ?? liveClass.Teacher.UserName) : "Yoxdur";
                ViewBag.RoomId = liveClass.RoomId;
                // Will check teacher status below properly
            }

            var userId = Guid.NewGuid().ToString();
            var fullName = "Guest " + new Random().Next(100, 999);

            if (User.Identity?.IsAuthenticated == true)
            {
                var appUser = await _userManager.GetUserAsync(User);
                if (appUser != null)
                {
                    userId = appUser.Id;
                    fullName = !string.IsNullOrEmpty(appUser.FullName) ? appUser.FullName : appUser.UserName;
                    
                    if (liveClass != null && liveClass.TeacherId == appUser.Id)
                    {
                        isTeacher = true; // Sadece o d?rsi yaradan mü?llim admin ola bil?r
                    }
                }
            }

            ViewBag.IsTeacher = isTeacher;
            ViewBag.UserId = userId;
            ViewBag.FullName = fullName;

            return View();
        }
    }
}
