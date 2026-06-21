using Academy.Data;
using Academy.Services.Interfaces;
using Academy.ViewModels.AboutUs;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Academy.ViewComponents.Advance
{
    public class AdvanceViewComponent : ViewComponent
    {
        private readonly IAboutUsService _aboutUsService;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public AdvanceViewComponent(IAboutUsService aboutUsService, IMapper mapper, AppDbContext context)
        {
            _mapper = mapper;
            _aboutUsService = aboutUsService;
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var about = await _aboutUsService.GetAllAsync();
            var abouts = about.FirstOrDefault();
            var aboutVM = _mapper.Map<AboutUsVM>(abouts);

            // Load first statistic for the floating badge
            var stat = await _context.Statistics.OrderBy(s => s.Id).FirstOrDefaultAsync();
            ViewBag.BadgeStat = stat;

            return View(aboutVM);
        }
    }
}
