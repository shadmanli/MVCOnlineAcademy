using Academy.Data;
using Academy.Models;
using Academy.Services.Interfaces;
using Academy.ViewModels.CourseRequirement;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Academy.Services
{
    public class CourseRequirementService : ICourseRequirementService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CourseRequirementService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CourseRequirementVM>> GetAllAsync()
        {
            var data = await _context.CourseRequirements
                .Include(x => x.Course)
                .ToListAsync();

            return _mapper.Map<IEnumerable<CourseRequirementVM>>(data);
        }

        public async Task CreateAsync(CourseRequirementCreateVM model)
        {
            var data = _mapper.Map<CourseRequirement>(model);
            await _context.CourseRequirements.AddAsync(data);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _context.CourseRequirements.FindAsync(id);
            if (data == null) return;

            _context.CourseRequirements.Remove(data);
            await _context.SaveChangesAsync();
        }

        public async Task<CourseRequirementEditVM> GetEditAsync(int id)
        {
            var data = await _context.CourseRequirements.FindAsync(id);

            return new CourseRequirementEditVM
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
        public async Task<CourseRequirementDetailVM> GetByIdAsync(int id)
        {
            var data = await _context.CourseRequirements
                .Include(x => x.Course)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (data == null) return null;

            return new CourseRequirementDetailVM
            {
                Id = data.Id,
                Text = data.Text,
                CourseName = data.Course.Title
            };
        }
        public async Task EditAsync(CourseRequirementEditVM model)
        {
            var data = await _context.CourseRequirements.FindAsync(model.Id);

            data.Text = model.Text;
            data.CourseId = model.CourseId;

            await _context.SaveChangesAsync();
        }

        
    }
}
