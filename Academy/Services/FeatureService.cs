using Academy.Data;
using Academy.Models;
using Academy.Services.Interfaces;
using Academy.ViewModels.Feature;
using Academy.ViewModels.FeatureVM;
using AutoMapper;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
            var data = _mapper.Map<Feature>(model);
            await _context.Feature.AddAsync(data);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Feature feature)
        {
            var data  = await _context.Feature.FindAsync(feature.Id);
             _context.Feature.Remove(data);
                await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<FeatureVM>> GetAllAsync()
        {
            var data = await _context.Feature.ToListAsync();
            var featureVMs = _mapper.Map<IEnumerable<FeatureVM>>(data);
            return featureVMs;
        }

        public async Task<Feature> GetByIdAsync(int id)
        {
            var data = await _context.Feature.FindAsync(id);
            
            return data;
        }
        public async Task<FeatureEditVM> GetByIdForEditAsync(int id)
        {
            var data = await _context.Feature.FindAsync(id);
            if (data == null) return null;

            return _mapper.Map<FeatureEditVM>(data);
        }
        public async Task UpdateAsync(FeatureEditVM model)
        {
            var data = await _context.Feature.FindAsync(model.Id);
            if (data == null) return;

            data.Title = model.Title;
            data.Description = model.Description;

            await _context.SaveChangesAsync();
        }
    }
}
