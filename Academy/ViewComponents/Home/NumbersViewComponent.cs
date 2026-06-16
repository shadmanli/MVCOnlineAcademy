using Academy.Data;
using Academy.Services.Interfaces;
using Academy.ViewModels.Statistic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Academy.ViewComponents.Home
{
    public class NumbersViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public NumbersViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Real hesablama
            int studentCount = await _context.Users.CountAsync();
            int courseCount = await _context.Courses.CountAsync();
            int instructorCount = await _context.Instructors.CountAsync();
            int currentLive = await _context.LiveClasses.Where(lc => lc.Status == Academy.Models.LiveSessionStatus.Live).CountAsync();

            var data = new List<StatisticVM>
            {
                new StatisticVM { Title = "Qeydiyyatdan keçən Tələbə", Count = studentCount > 0 ? studentCount : 0 },
                new StatisticVM { Title = "Hazırki Kurslar", Count = courseCount > 0 ? courseCount : 0 },
                new StatisticVM { Title = "Peşəkar Təlimçilər", Count = instructorCount > 0 ? instructorCount : 0 },
                new StatisticVM { Title = "Aktiv Canlı Dərslər", Count = currentLive }
            };

            return View(data);
        }
    }
}
