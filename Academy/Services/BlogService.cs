using Academy.Data;
using Academy.Models;
using Academy.Services.Interfaces;
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

        public async  Task<IEnumerable<BlogVM>> GetAllAsync()
        {

            var blog = await _context.Blog.ToListAsync();
            return _mapper.Map<IEnumerable<BlogVM>>(blog);


        }
    }
}
