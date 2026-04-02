using Academy.Data;
using Academy.Models;
using Academy.Services.Interfaces;
using Academy.ViewModels.Article;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Academy.Services
{
    public class ArticleService : IArticleService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public ArticleService(AppDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }

        public async Task CreateAsync(ArticleCreateVM model)
        {
            string folderPath = Path.Combine(_env.WebRootPath, "uploads", "article");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string fileName = null;

            if (model.Image != null)
            {
                fileName = $"{Guid.NewGuid()}_{model.Image.FileName}";
                string path = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await model.Image.CopyToAsync(stream);
                }
            }

            var data = _mapper.Map<Article>(model);

            data.Image = fileName; // ✅ əsas hissə
            data.CreatedAt = DateTime.Now;
            data.TopicId = model.TopicId;

            await _context.Articles.AddAsync(data);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _context.Articles.FirstOrDefaultAsync(x => x.Id == id);

            if (data == null) return;

            // ❗ istəyirsənsə file da silinsin
            if (!string.IsNullOrEmpty(data.Image))
            {
                string path = Path.Combine(_env.WebRootPath, "uploads", "article", data.Image);

                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }

            _context.Articles.Remove(data);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ArticleVM>> GetAllAsync()
        {
            var data = await _context.Articles
                .Include(x => x.Topic)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ArticleVM>>(data);
        }

        public async Task<Article> GetByIdAsync(int id)
        {
            return await _context.Articles.FindAsync(id);
        }

        public async Task UpdateAsync(ArticleEditVM model)
        {
            var data = await _context.Articles.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (data == null) return;

            string folderPath = Path.Combine(_env.WebRootPath, "uploads", "article");

            if (model.Image != null)
            {
              
                if (!string.IsNullOrEmpty(data.Image))
                {
                    string oldPath = Path.Combine(folderPath, data.Image);
                    if (File.Exists(oldPath))
                    {
                        File.Delete(oldPath);
                    }
                }

               


                string fileName = $"{Guid.NewGuid()}_{model.Image.FileName}";
                string newPath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(newPath, FileMode.Create))
                {
                    await model.Image.CopyToAsync(stream);
                }

                data.Image = fileName;
            }

            data.Description = model.Description;
            data.Text = model.Text;
            data.TopicId = model.TopicId;

            await _context.SaveChangesAsync();
        }
    }
}
