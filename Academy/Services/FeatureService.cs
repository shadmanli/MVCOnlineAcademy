using Academy.Data;
using Academy.Models;
using Academy.Services.Interfaces;
using Academy.ViewModels.Feature;
using Academy.ViewModels.FeatureVM;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Academy.Services
{
    public class FeatureService : IFeatureService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public FeatureService(AppDbContext context,IMapper mapper)
        {
           _context = context;
            _mapper = mapper;
        }

        public async Task CreateAsync(FeatureCreateVM model)
        {
            
        }

        public async Task<IEnumerable<FeatureVM>> GetAllAsync()
        {
            var data = await _context.Feature.ToListAsync();
            var featureVMs = _mapper.Map<IEnumerable<FeatureVM>>(data);
            return featureVMs;
        }
    }
}
