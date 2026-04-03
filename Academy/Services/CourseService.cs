using Academy.Data;
using Academy.Models;
using Academy.Services.Interfaces;
using Academy.ViewModels.Course;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Academy.Services
{
    public class CourseService : ICourseService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        public CourseService(AppDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }

        public async Task<IEnumerable<CourseVM>> GetAllAsync()
        {
            var data = await _context.Courses
                .Include(x => x.Language)
                .Include(x => x.Category)
                .Include(x => x.Instructor)
                .ToListAsync();

            return _mapper.Map<IEnumerable<CourseVM>>(data);
        }

        public async Task CreateAsync(CourseCreateVM model)
        {
            string folder = Path.Combine(_env.WebRootPath, "uploads/course");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string fileName = null;

            if (model.Image != null)
            {
                fileName = Guid.NewGuid() + "_" + model.Image.FileName;
                string path = Path.Combine(folder, fileName);

                using var stream = new FileStream(path, FileMode.Create);
                await model.Image.CopyToAsync(stream);
            }

            var data = _mapper.Map<Course>(model);

            data.ImageUrl = fileName;
            data.CreatedDate = DateTime.Now;

            await _context.Courses.AddAsync(data);
            await _context.SaveChangesAsync();
        }

        public async Task<CourseDetailVM> GetByIdAsync(int id)
        {
            var data = await _context.Courses
                .Include(x => x.Language)
                .Include(x => x.Category)
                .Include(x => x.Instructor)
                .FirstOrDefaultAsync(x => x.Id == id);

            return _mapper.Map<CourseDetailVM>(data);
        }

     
        public async Task DeleteAsync(int id)
        {
            var data = await _context.Courses.FindAsync(id);

            _context.Courses.Remove(data);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CourseEditVM model)
        {
            var data = await _context.Courses
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (data == null) return;

            // Şəkil dəyişilibsə
            if (model.ImageFile != null)
            {
                string folder = Path.Combine(_env.WebRootPath, "uploads/course");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                // köhnə şəkili sil
                if (!string.IsNullOrEmpty(data.ImageUrl))
                {
                    string oldPath = Path.Combine(folder, data.ImageUrl);
                    if (File.Exists(oldPath))
                        File.Delete(oldPath);
                }

                string fileName = Guid.NewGuid() + "_" + model.ImageFile.FileName;
                string newPath = Path.Combine(folder, fileName);

                using var stream = new FileStream(newPath, FileMode.Create);
                await model.ImageFile.CopyToAsync(stream);

                data.ImageUrl = fileName;
            }

           
            data.Title = model.Title;
            data.Description = model.Description;
            data.Price = model.Price;
            data.Duration = model.Duration;
            data.StudentCount = model.StudentCount;

            data.LanguageId = model.LanguageId;
            data.CategoryId = model.CategoryId;
            data.InstructorId = model.InstructorId;

            data.UpdatedDate = DateTime.Now;

            await _context.SaveChangesAsync();
        }
    }
}
