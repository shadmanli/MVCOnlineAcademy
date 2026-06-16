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
        private readonly IWebHostEnvironment _env;

        public InstructorService(AppDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }

        private async Task<string?> SaveImageAsync(IFormFile? file)
        {
            if (file == null || file.Length == 0) return null;
            var folder = Path.Combine(_env.WebRootPath, "uploads", "instructors");
            Directory.CreateDirectory(folder);
            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            using var stream = new FileStream(Path.Combine(folder, fileName), FileMode.Create);
            await file.CopyToAsync(stream);
            return fileName;
        }

        public async Task<IEnumerable<InstructorVM>> GetAllAsync()
        {
            var data = await _context.Instructors.ToListAsync();
            return _mapper.Map<IEnumerable<InstructorVM>>(data);
        }

        public async Task CreateAsync(InstructorCreateVM model)
        {
            var data = _mapper.Map<Instructor>(model);
            data.Image = await SaveImageAsync(model.Image);
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
            var data = await _context.Instructors.FindAsync(id);
            if (data == null) return;

            // Şəkili sil
            if (!string.IsNullOrEmpty(data.Image))
            {
                var path = Path.Combine(_env.WebRootPath, "uploads", "instructors", data.Image);
                if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
            }

            // Kursların instructor FK-ni başqa dəyərə qoymadan birbaşa sil
            // (ON DELETE SET NULL üçün migration lazımdır, buna görə kursları saxlayırıq)
            await _context.Database.ExecuteSqlRawAsync(
                "UPDATE Courses SET InstructorId = NULL WHERE InstructorId = {0}", id);

            _context.Instructors.Remove(data);
            await _context.SaveChangesAsync();
        }

        public async Task<InstructorEditVM> GetEditByIdAsync(int id)
        {
            var data = await _context.Instructors.FindAsync(id);
            if (data == null) return null;
            return new InstructorEditVM
            {
                Id = data.Id,
                FullName = data.FullName,
                Title = data.Title,
                Bio = data.Bio,
                ExistingImage = data.Image
            };
        }

        public async Task EditAsync(InstructorEditVM model)
        {
            var data = await _context.Instructors.FindAsync(model.Id);
            if (data == null) return;

            data.FullName = model.FullName;
            data.Title = model.Title;
            data.Bio = model.Bio;

            if (model.Image != null)
            {
                if (!string.IsNullOrEmpty(data.Image))
                {
                    var old = Path.Combine(_env.WebRootPath, "uploads", "instructors", data.Image);
                    if (System.IO.File.Exists(old)) System.IO.File.Delete(old);
                }
                data.Image = await SaveImageAsync(model.Image);
            }

            await _context.SaveChangesAsync();
        }
    }
}
