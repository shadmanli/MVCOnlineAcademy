using Academy.Data;
using Academy.Models;
using Academy.Services.Interfaces;
using Academy.ViewModels.CourseName;
using Microsoft.EntityFrameworkCore;

namespace Academy.Services
{
    public class CourseNameService : ICourseNameService
    {
        private readonly AppDbContext _context;

        public CourseNameService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CourseNameVM>> GetAllAsync()
        {
            return await _context.CourseNames
                .Include(cn => cn.CourseNameCategories)
                    .ThenInclude(cnc => cnc.Category)
                .OrderBy(cn => cn.Name)
                .Select(cn => new CourseNameVM
                {
                    Id          = cn.Id,
                    Name        = cn.Name,
                    Description = cn.Description,
                    IsActive    = cn.IsActive,
                    CategoryIds   = cn.CourseNameCategories.Select(c => c.CategoryId).ToList(),
                    CategoryNames = cn.CourseNameCategories.Select(c => c.Category.Name).ToList()
                })
                .ToListAsync();
        }

        public async Task<CourseNameVM?> GetByIdAsync(int id)
        {
            var cn = await _context.CourseNames
                .Include(x => x.CourseNameCategories)
                    .ThenInclude(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (cn == null) return null;

            return new CourseNameVM
            {
                Id          = cn.Id,
                Name        = cn.Name,
                Description = cn.Description,
                IsActive    = cn.IsActive,
                CategoryIds   = cn.CourseNameCategories.Select(c => c.CategoryId).ToList(),
                CategoryNames = cn.CourseNameCategories.Select(c => c.Category.Name).ToList()
            };
        }

        public async Task<List<CourseNameVM>> GetActiveNamesAsync()
        {
            return await _context.CourseNames
                .Where(cn => cn.IsActive)
                .OrderBy(cn => cn.Name)
                .Select(cn => new CourseNameVM
                {
                    Id   = cn.Id,
                    Name = cn.Name
                })
                .ToListAsync();
        }

        public async Task CreateAsync(CourseNameCreateVM model)
        {
            var entity = new CourseName
            {
                Name        = model.Name,
                Description = model.Description,
                IsActive    = model.IsActive,
                CreatedAt   = DateTime.Now
            };

            if (model.CategoryIds.Any())
            {
                entity.CourseNameCategories = model.CategoryIds
                    .Select(cid => new CourseNameCategory
                    {
                        CategoryId = cid
                    }).ToList();
            }

            _context.CourseNames.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, CourseNameEditVM model)
        {
            var entity = await _context.CourseNames
                .Include(cn => cn.CourseNameCategories)
                .FirstOrDefaultAsync(cn => cn.Id == id);

            if (entity == null) return;

            entity.Name        = model.Name;
            entity.Description = model.Description;
            entity.IsActive    = model.IsActive;

            // Kateqoriyaları yenilə
            entity.CourseNameCategories.Clear();
            if (model.CategoryIds.Any())
            {
                entity.CourseNameCategories = model.CategoryIds
                    .Select(cid => new CourseNameCategory
                    {
                        CourseNameId = entity.Id,
                        CategoryId   = cid
                    }).ToList();
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.CourseNames.FindAsync(id);
            if (entity == null) return;
            _context.CourseNames.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<CourseNameCategoryDto>> GetCategoriesByNameIdAsync(int courseNameId)
        {
            return await _context.CourseNameCategories
                .Where(cnc => cnc.CourseNameId == courseNameId)
                .Select(cnc => new CourseNameCategoryDto
                {
                    Id   = cnc.Category.Id,
                    Name = cnc.Category.Name
                })
                .ToListAsync();
        }
    }
}
