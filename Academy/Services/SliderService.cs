using Academy.Data;
using Academy.Models;
using Academy.Services.Interfaces;
using Academy.ViewModels.Slider;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Academy.Services
{
    public class SliderService : ISliderService
    {

        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;



        public SliderService(AppDbContext context, IWebHostEnvironment env, IMapper mapper)
        {
            _context = context;
            _env = env;
            _mapper = mapper;
        }

        public async Task CreateAsync(SliderCreateVM slider)
        {
            string folderPath = Path.Combine(_env.WebRootPath, "uploads", "sliders");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            string fileName = $"{Guid.NewGuid()}_{slider.Image.FileName}";
            string path = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await slider.Image.CopyToAsync(stream);
            }

            var sliders = _mapper.Map<Slider>(slider);
            sliders.Image = fileName;
            sliders.CreatedAt = DateTime.Now;
            await _context.Sliders.AddAsync(sliders);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Slider slider)
        {
            var path = Path.Combine(_env.WebRootPath, "uploads", "sliders", slider.Image);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(SliderEditVM model)
        {
            var slider = await GetByIdAsync(model.Id);

            slider.Title = model.Title;

            slider.Description = model.Description;

            if (model.Photo != null && model.Photo.Length > 0)
            {
                string folderPath = Path.Combine(_env.WebRootPath, "uploads", "sliders");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                string oldPath = Path.Combine(folderPath, slider.Image);
                if (File.Exists(oldPath))
                    File.Delete(oldPath);

                string newFileName = $"{Guid.NewGuid()}_{model.Photo.FileName}";
                string newPath = Path.Combine(folderPath, newFileName);
                using (var stream = new FileStream(newPath, FileMode.Create))
                {
                    await model.Photo.CopyToAsync(stream);
                }
                slider.Image = newFileName;
            }
            _context.Sliders.Update(slider);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Slider>> GetAllAsync()
        {
            return await _context.Sliders.OrderByDescending(m => m.Id).ToListAsync();
        }

        public async Task<Slider> GetByIdAsync(int id)
        {
            var slider = await _context.Sliders.FindAsync(id);
            return slider;


        }

    }
}
