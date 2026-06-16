using Academy.Data;
using Academy.Models;
using Academy.Services.Interfaces;
using Academy.ViewModels.Feature;
using Academy.ViewModels.FeatureVM;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Academy.Services
{
    public class FeatureService : IFeatureService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public FeatureService(AppDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }

        private async Task<string?> SaveImageAsync(IFormFile? file)
        {
            if (file == null || file.Length == 0) return null;

            var folder = Path.Combine(_env.WebRootPath, "uploads", "features");
            Directory.CreateDirectory(folder);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var fullPath = Path.Combine(folder, fileName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return fileName;
        }

        public async Task CreateAsync(FeatureCreateVM model)
        {
            var data = _mapper.Map<Feature>(model);
            data.Image = await SaveImageAsync(model.Image);
            await _context.Feature.AddAsync(data);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Feature feature)
        {
            var data = await _context.Feature.FindAsync(feature.Id);
            if (data == null) return;

            // Şəkili sil
            if (!string.IsNullOrEmpty(data.Image))
            {
                var path = Path.Combine(_env.WebRootPath, "uploads", "features", data.Image);
                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
            }

            _context.Feature.Remove(data);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<FeatureVM>> GetAllAsync()
        {
            var data = await _context.Feature.ToListAsync();
            return _mapper.Map<IEnumerable<FeatureVM>>(data);
        }

        public async Task<Feature> GetByIdAsync(int id)
        {
            return await _context.Feature.FindAsync(id);
        }

        public async Task<FeatureEditVM> GetByIdForEditAsync(int id)
        {
            var data = await _context.Feature.FindAsync(id);
            if (data == null) return null;

            var model = _mapper.Map<FeatureEditVM>(data);
            model.ExistingImage = data.Image;
            return model;
        }

        public async Task UpdateAsync(FeatureEditVM model)
        {
            var data = await _context.Feature.FindAsync(model.Id);
            if (data == null) return;

            data.Title = model.Title;
            data.Description = model.Description;

            if (model.Image != null)
            {
                // Köhnə şəkili sil
                if (!string.IsNullOrEmpty(data.Image))
                {
                    var oldPath = Path.Combine(_env.WebRootPath, "uploads", "features", data.Image);
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }
                data.Image = await SaveImageAsync(model.Image);
            }

            await _context.SaveChangesAsync();
        }
    }
}
