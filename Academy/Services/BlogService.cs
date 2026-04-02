using Academy.Data;
using Academy.Models;
using Academy.Services.Interfaces;
using Academy.ViewModels.AboutUs;
using Academy.ViewModels.Blog;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Academy.Services
{
    public class BlogService : IBlogService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;
        public BlogService(AppDbContext context,IWebHostEnvironment env,IMapper mapper)
        {
            _context = context;
            _env = env;
            _mapper = mapper;
        }

        public   async Task CreateAsycn(BlogCreateVM model)
        {
            string folderPath = Path.Combine(_env.WebRootPath, "uploads", "blog");
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
            var aboutusData = _mapper.Map<Blog>(model);
            aboutusData.Image = fileName;
            aboutusData.CreatedAt = DateTime.Now;

            await _context.Blog.AddAsync(aboutusData);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _context.Blog.FindAsync(id);
            var path = Path.Combine(_env.WebRootPath, "uploads", "blog", data.Image);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            _context.Blog.Remove(data);
            await _context.SaveChangesAsync();
        }

        

        public async  Task<IEnumerable<BlogVM>> GetAllAsync()
        {

            var blog = await _context.Blog.ToListAsync();
            return _mapper.Map<IEnumerable<BlogVM>>(blog);


        }

        public async Task<BlogDetailVM> GetByIdAsync(int id)
        {
           var data = await _context.Blog.FindAsync(id);
            return _mapper.Map<BlogDetailVM>(data);
        }


        public async Task UpdateAsync(int id, BlogEditVM model)
        {
            var data = await _context.Blog.FindAsync(id);
            if (data == null) return;

            if (model.Image != null)
            {
                string folderPath = Path.Combine(_env.WebRootPath, "uploads", "blog");

              
                string oldPath = Path.Combine(folderPath, data.Image);
                if (File.Exists(oldPath))
                {
                    File.Delete(oldPath);
                }

              
                string fileName = $"{Guid.NewGuid()}_{model.Image.FileName}";
                string newPath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(newPath, FileMode.Create))
                {
                    await model.Image.CopyToAsync(stream);
                }

                data.Image = fileName;
            }

            data.Title = model.Title;
            data.Description = model.Description;
            data.Name = model.Name;

            await _context.SaveChangesAsync();
        }
    }
}
