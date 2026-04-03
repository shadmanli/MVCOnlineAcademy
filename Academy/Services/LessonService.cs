using Academy.Data;
using Academy.Models;
using Academy.Services.Interfaces;
using Academy.ViewModels.Lesson;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Academy.Services
{
    public class LessonService : ILessonService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public LessonService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LessonVM>> GetAllAsync()
        {
            var data = await _context.Lessons
                .Include(x => x.Course)
                .ToListAsync();

            return _mapper.Map<IEnumerable<LessonVM>>(data);
        }

        public async Task CreateAsync(LessonCreateVM model)
        {
            var data = _mapper.Map<Lesson>(model);

            data.CreatedAt = DateTime.Now;

            await _context.Lessons.AddAsync(data);
            await _context.SaveChangesAsync();
        }

        public async Task<LessonDetailVM> GetByIdAsync(int id)
        {
            var data = await _context.Lessons
                .Include(x => x.Course)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (data == null) return null;

            return _mapper.Map<LessonDetailVM>(data);
        }

      
        public async Task DeleteAsync(int id)
        {
            var data = await _context.Lessons.FindAsync(id);

            if (data == null) return;

            _context.Lessons.Remove(data);
            await _context.SaveChangesAsync();
        }


        public async Task<LessonEditVM> GetEditByIdAsync(int id)
        {
            var data = await _context.Lessons.FindAsync(id);
            if (data == null) return null;

            return new LessonEditVM
            {
                Id = data.Id,
                Title = data.Title,
                CourseId = data.CourseId,
                Courses = await _context.Courses
                    .Select(x => new SelectListItem
                    {
                        Text = x.Title,
                        Value = x.Id.ToString()
                    }).ToListAsync()
            };
        }

        public async Task EditAsync(LessonEditVM model)
        {
            var data = await _context.Lessons.FindAsync(model.Id);
            if (data == null) return;

            data.Title = model.Title;
            data.CourseId = model.CourseId;
          

            await _context.SaveChangesAsync();
        }
    }
}
