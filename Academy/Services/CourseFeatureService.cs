using Academy.Data;
using Academy.Models;
using Academy.Services.Interfaces;
using Academy.ViewModels.CourseFeature;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Academy.Services
{
    public class CourseFeatureService : ICourseFeatureService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CourseFeatureService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CourseFeatureVM>> GetAllAsync()
        {
            var data = await _context.CourseFeatures
                .Include(x => x.Course)
                .ToListAsync();

            return _mapper.Map<IEnumerable<CourseFeatureVM>>(data);
        }

        public async Task CreateAsync(CourseFeatureCreateVM model)
        {
            var data = _mapper.Map<CourseFeature>(model);
            data.CreatedAt = DateTime.Now;

            await _context.CourseFeatures.AddAsync(data);
            await _context.SaveChangesAsync();
        }

        public async Task<CourseFeatureDetailVM> GetByIdAsync(int id)
        {
            var data = await _context.CourseFeatures
                .Include(x => x.Course)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (data == null) return null;

            return _mapper.Map<CourseFeatureDetailVM>(data);
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _context.CourseFeatures.FindAsync(id);
            if (data == null) return;

            _context.CourseFeatures.Remove(data);
            await _context.SaveChangesAsync();
        }

        public async Task<CourseFeatureEditVM> GetEditByIdAsync(int id)
        {
            var data = await _context.CourseFeatures.FindAsync(id);
            if (data == null) return null;

            return new CourseFeatureEditVM
            {
                Id = data.Id,
                Text = data.Text,
                CourseId = data.CourseId,
                Courses = await _context.Courses
                    .Select(x => new SelectListItem
                    {
                        Text = x.Title,
                        Value = x.Id.ToString()
                    }).ToListAsync()
            };
        }

        public async Task EditAsync(CourseFeatureEditVM model)
        {
            var data = await _context.CourseFeatures.FindAsync(model.Id);
            if (data == null) return;

            data.Text = model.Text;
            data.CourseId = model.CourseId;

            await _context.SaveChangesAsync();
        }
    }
}
