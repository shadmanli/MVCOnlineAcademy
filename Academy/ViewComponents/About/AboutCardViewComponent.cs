using Academy.Data;
using Academy.Services.Interfaces;
using Academy.ViewModels.FeatureVM;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Academy.ViewComponents.About
{
    public class AboutCardViewComponent : ViewComponent
    {
        private readonly IFeatureService _featureService;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public AboutCardViewComponent(IFeatureService featureService, IMapper mapper, AppDbContext context)
        {
            _featureService = featureService;
            _mapper = mapper;
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var data = await _featureService.GetAllAsync();
            var featureVMs = _mapper.Map<IEnumerable<FeatureVM>>(data);

            // Birbaşa Statistics cədvəlindən al
            var statistics = await _context.Statistics
                .OrderBy(s => s.Id)
                .Take(3)
                .ToListAsync();

            ViewBag.Statistics = statistics;

            return View(featureVMs);
        }
    }
}
