using Academy.Data;
using Academy.Models;
using Academy.Services.Interfaces;
using Academy.ViewModels.Banner;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Academy.Services
{
    public class BannerService : IBannerService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public BannerService(AppDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }

        public async  Task CreateAsync(BannerCreateVM model)
        {
            string folderPath = Path.Combine(_env.WebRootPath, "uploads",  "banner");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            string fileName = $"{Guid.NewGuid()}_{model.Image.FileName}";
            string path = Path.Combine(folderPath, fileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await model.Image.CopyToAsync(stream);
            }

            var bannerData = _mapper.Map<Banner>(model);
            bannerData.Image = fileName;
            _context.Banners.Add(bannerData);
            await _context.SaveChangesAsync();
        }

        public  async Task DeleteAsync(int id)
        {
            var data = await _context.Banners.FindAsync(id);
            _context.Banners.Remove(data);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<BannerVM>> GetAllAsync()
        {
            var data = await _context.Banners.ToListAsync();
            var vm = _mapper.Map<IEnumerable<BannerVM>>(data);
            return vm;
        }

        public async  Task<BannerDetailVM> GetByIdAsync(int id)
        {
           var data = await _context.Banners.FindAsync(id);
          
            var vm = _mapper.Map<BannerDetailVM>(data);
            return vm;
        }
    }
}
