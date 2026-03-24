using Academy.Data;
using Academy.Models;
using Academy.Services.Interfaces;
using Academy.ViewModels.ImpactItem;
using Academy.ViewModels.ImpactSection;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Academy.Services
{
    public class ImpactSectionService : IImpactSectionService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public ImpactSectionService(AppDbContext context,IMapper mapper)
        {
            _context = context;
             _mapper = mapper;  
        }

        public async Task CreateAsync(ImpactSectionCreateVM model)
        {
           var data  = _mapper.Map<ImpactSection>(model);
            await _context.ImpactSections.AddAsync(data);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _context.ImpactSections
                .Include(x => x.ImpactItems)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (data == null) return;

          
            if (data.ImpactItems != null && data.ImpactItems.Any())
            {
                _context.ImpactItems.RemoveRange(data.ImpactItems);
            }

        
            _context.ImpactSections.Remove(data);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ImpactSectionVM>> GetAllAsync()
        {
            var impactSections = await _context.ImpactSections
                .Include(x => x.ImpactItems)  
                .ToListAsync();

            var impactSectionVMs = _mapper.Map<IEnumerable<ImpactSectionVM>>(impactSections);

            return impactSectionVMs;
        }

        public async Task<ImpactSectionDetailVM> GetByIdAsync(int id)
        {
            var data = await _context.ImpactSections
               .Include(x => x.ImpactItems)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (data == null) return null;

            return _mapper.Map<ImpactSectionDetailVM>(data);
        }
    }
}
