using Academy.Data;
using Academy.Models;
using Academy.Services.Interfaces;
using Academy.ViewModels.Language;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Academy.Services
{
    public class LanguageService : ILanguageService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public LanguageService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LanguageVM>> GetAllAsync()
        {
            var data = await _context.Languages
                .Include(x => x.Courses)
                .ToListAsync();

            return _mapper.Map<IEnumerable<LanguageVM>>(data);
        }

        public async Task CreateAsync(LanguageCreateVM model)
        {
            var data = _mapper.Map<Language>(model);

            data.CreatedAt = DateTime.Now;

            await _context.Languages.AddAsync(data);
            await _context.SaveChangesAsync();
        }

        public async Task<LanguageDetailVM> GetByIdAsync(int id)
        {
            var data = await _context.Languages
                .Include(x => x.Courses)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (data == null) return null;

            return _mapper.Map<LanguageDetailVM>(data);
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _context.Languages
                .Include(x => x.Courses)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (data == null) return;

           
            if (data.Courses != null && data.Courses.Any())
            {
                _context.Courses.RemoveRange(data.Courses);
            }

            _context.Languages.Remove(data);
            await _context.SaveChangesAsync();
        }






        public async Task<LanguageEditVM> GetEditByIdAsync(int id)
        {
            var data = await _context.Languages.FindAsync(id);
            if (data == null) return null;

            return _mapper.Map<LanguageEditVM>(data);
        }

        public async Task EditAsync(LanguageEditVM model)
        {
            var data = await _context.Languages.FindAsync(model.Id);
            if (data == null) return;

            data.Name = model.Name;
           

            await _context.SaveChangesAsync();
        }
    }
}
