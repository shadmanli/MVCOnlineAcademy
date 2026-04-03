using Academy.Data;
using Academy.Models;
using Academy.Services.Interfaces;
using Academy.ViewModels.Category;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Academy.Services
{

    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CategoryService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryVM>> GetAllAsync()
        {
            var data = await _context.Categories
                .Include(c => c.Courses)
                .ToListAsync();
            return _mapper.Map<IEnumerable<CategoryVM>>(data);
        }

        public async Task CreateAsync(CategoryCreateVM model)
        {
            var data = _mapper.Map<Category>(model);
           
            await _context.Categories.AddAsync(data);
            await _context.SaveChangesAsync();
        }

        public async Task<CategoryDetailVM> GetByIdAsync(int id)
        {
            var data = await _context.Categories
                .Include(c => c.Courses)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (data == null) return null;
            return _mapper.Map<CategoryDetailVM>(data);
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _context.Categories
                .Include(c => c.Courses)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (data == null) return;
            
            if (data.Courses != null && data.Courses.Any())
                _context.Courses.RemoveRange(data.Courses);

            _context.Categories.Remove(data);
            await _context.SaveChangesAsync();
        }

        public async Task<CategoryEditVM> GetEditByIdAsync(int id)
        {
            var data = await _context.Categories.FindAsync(id);
            if (data == null) return null;

            return new CategoryEditVM
            {
                Id = data.Id,
                Name = data.Name
            };
        }

        public async Task EditAsync(CategoryEditVM model)
        {
            var data = await _context.Categories.FindAsync(model.Id);
            if (data == null) return;

            data.Name = model.Name;

            await _context.SaveChangesAsync();
        }


    }
}
