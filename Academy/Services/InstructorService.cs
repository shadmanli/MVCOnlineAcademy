using Academy.Data;
using Academy.Models;
using Academy.Services.Interfaces;
using Academy.ViewModels.Instructor;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Academy.Services
{
    public class InstructorService : IInstructorService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public InstructorService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<InstructorVM>> GetAllAsync()
        {
            var data = await _context.Instructors.ToListAsync();
            return _mapper.Map<IEnumerable<InstructorVM>>(data);
        }

        public async Task CreateAsync(InstructorCreateVM model)
        {
            var data = _mapper.Map<Instructor>(model);

            data.CreatedAt = DateTime.Now;

            await _context.Instructors.AddAsync(data);
            await _context.SaveChangesAsync();
        }

        public async Task<InstructorDetailVM> GetByIdAsync(int id)
        {
            var data = await _context.Instructors
                .Include(x => x.Courses)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (data == null) return null;

            return _mapper.Map<InstructorDetailVM>(data);
        }

   
        public async Task DeleteAsync(int id)
        {
            var data = await _context.Instructors
                .Include(x => x.Courses)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (data == null) return;

          
            if (data.Courses != null && data.Courses.Any())
            {
                _context.Courses.RemoveRange(data.Courses);
            }

            _context.Instructors.Remove(data);
            await _context.SaveChangesAsync();
        }
    }
}
