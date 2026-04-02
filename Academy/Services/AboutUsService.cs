using Academy.Data;
using Academy.Models;
using Academy.Services.Interfaces;
using Academy.ViewModels.AboutUs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Academy.Services
{
    public class AboutUsService : IAboutUsService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        public AboutUsService(AppDbContext context,IMapper mapper,IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }

        public async Task CreateAsync(AboutUsCreateVM aboutUs)
        {
            string folderPath = Path.Combine(_env.WebRootPath, "uploads", "aboutus");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            string fileName = $"{Guid.NewGuid()}_{aboutUs.Image.FileName}";
            string path = Path.Combine(folderPath, fileName);
            using (var stream = new FileStream(path , FileMode.Create))
            {
                await aboutUs.Image.CopyToAsync(stream);
            }
            var aboutusData = _mapper.Map<AboutUs>(aboutUs);
            aboutusData.Image = fileName;
            aboutusData.CreatedAt = DateTime.Now;

            await _context.AboutUs.AddAsync(aboutusData);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(AboutUs aboutus)
        {
           var data = await _context.AboutUs.FindAsync(aboutus.Id);
           ;
            var path = Path.Combine(_env.WebRootPath, "uploads", "aboutus", data.Image);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            _context.AboutUs.Remove(data);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<AboutUs>> GetAllAsync()
        {
            return await _context.AboutUs.ToListAsync();
        }

        public async Task<AboutUs> GetByIdAsync(int id)
        {
           var about = await _context.AboutUs.FindAsync(id);
            return about;
        }


        public async Task EditAsync(AboutUsEditVM model)
        {
            var data = await _context.AboutUs.FindAsync(model.Id);

            if (data == null) return;

            string folderPath = Path.Combine(_env.WebRootPath, "uploads", "aboutus");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            if (!string.IsNullOrWhiteSpace(model.Title))
                data.Title = model.Title.Trim();

      
            if (!string.IsNullOrWhiteSpace(model.Description))
                data.Description = model.Description.Trim();

            if (!string.IsNullOrWhiteSpace(model.Phone))
                data.Phone = model.Phone.Trim();

        
            if (model.NewImage != null && model.NewImage.Length > 0)
            {
               
                if (!string.IsNullOrEmpty(data.Image))
                {
                    string oldPath = Path.Combine(folderPath, data.Image);

                    if (File.Exists(oldPath))
                        File.Delete(oldPath);
                }

              
                string extension = Path.GetExtension(model.NewImage.FileName);
                string newImageName = $"{Guid.NewGuid()}{extension}";
                string newPath = Path.Combine(folderPath, newImageName);

                using (FileStream stream = new FileStream(newPath, FileMode.Create))
                {
                    await model.NewImage.CopyToAsync(stream);
                }

                data.Image = newImageName;
            }

    
            await _context.SaveChangesAsync();
        }
    }
}
