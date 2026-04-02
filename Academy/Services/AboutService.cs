using Academy.Data;
using Academy.Models;
using Academy.Services.Interfaces;
using Academy.ViewModels.About;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Academy.Services
{
    public class AboutService : IAboutService
    {
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;
        public AboutService(IMapper mapper,IWebHostEnvironment env,AppDbContext context)
        {
            _context  = context;
            _mapper = mapper;
            _env = env;
        }

        public async Task CreateAsync(AboutCreateVM about)
        {
            string folderPath = Path.Combine(_env.WebRootPath, "uploads", "about");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

           
            string fileName = $"{Guid.NewGuid()}_{about.Image.FileName}";
            string filePath = Path.Combine(folderPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await about.Image.CopyToAsync(stream);
            }

            string videoFileName = null;
            if (about.Video != null)
            {
                videoFileName = $"{Guid.NewGuid()}_{about.Video.FileName}";
                string videoPath = Path.Combine(folderPath, videoFileName);
                using (var stream = new FileStream(videoPath, FileMode.Create))
                {
                    await about.Video.CopyToAsync(stream);
                }
            }

          
            var aboutData = _mapper.Map<About>(about);
            aboutData.Image = fileName;
            aboutData.Video = videoFileName;
            aboutData.CreatedAt = DateTime.Now;

            await _context.About.AddAsync(aboutData);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _context.About.FindAsync(id);

            if (data == null) return;

            string folderPath = Path.Combine(_env.WebRootPath, "uploads", "about");

            if (!string.IsNullOrEmpty(data.Image))
            {
                string imagePath = Path.Combine(folderPath, data.Image);
                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                }
            }

            if (!string.IsNullOrEmpty(data.Video))
            {
                string videoPath = Path.Combine(folderPath, data.Video);
                if (File.Exists(videoPath))
                {
                    File.Delete(videoPath);
                }
            }

            _context.About.Remove(data);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(AboutEditVM model)
        {
            var data = await _context.About.FindAsync(model.Id);

            if (data == null) return;

            string folderPath = Path.Combine(_env.WebRootPath, "uploads", "about");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

       
            if (model.NewImage != null && model.NewImage.Length > 0)
            {
        
                if (!string.IsNullOrEmpty(data.Image))
                {
                    string oldImagePath = Path.Combine(folderPath, data.Image);
                    if (File.Exists(oldImagePath))
                        File.Delete(oldImagePath);
                }

       
                string newImageName = $"{Guid.NewGuid()}_{model.NewImage.FileName}";
                string newImagePath = Path.Combine(folderPath, newImageName);
                using (FileStream stream = new FileStream(newImagePath, FileMode.Create))
                {
                    await model.NewImage.CopyToAsync(stream);
                }

                data.Image = newImageName;
            }

           
            if (model.NewVideo != null && model.NewVideo.Length > 0)
            {
         
                if (!string.IsNullOrEmpty(data.Video))
                {
                    string oldVideoPath = Path.Combine(folderPath, data.Video);
                    if (File.Exists(oldVideoPath))
                        File.Delete(oldVideoPath);
                }


                string newVideoName = $"{Guid.NewGuid()}_{model.NewVideo.FileName}";
                string newVideoPath = Path.Combine(folderPath, newVideoName);
                using (FileStream stream = new FileStream(newVideoPath, FileMode.Create))
                {
                    await model.NewVideo.CopyToAsync(stream);
                }

                data.Video = newVideoName;
            }

    
            data.Title = model.Title;
            data.Description = model.Description;

            await _context.SaveChangesAsync();
        }

        public async  Task<IEnumerable<AboutVM>> GetAllAsync()
        {
            var data = await _context.About.ToListAsync();
            var aboutVM = _mapper.Map<IEnumerable<AboutVM>>(data);
            return aboutVM;
        }

        public async  Task<AboutDetailVM> GetByIdAsync(int id)
        {
          var data = await _context.About.FindAsync(id);
            var aboutVM = _mapper.Map<AboutDetailVM>(data);
            return aboutVM;
        }

        public async Task<About> GetEntityByIdAsync(int id)
        {
            return await _context.About.FindAsync(id);
        }
    }
}
