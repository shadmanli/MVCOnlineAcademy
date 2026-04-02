using Academy.Data;
using Academy.Models;
using Academy.Services.Interfaces;
using Academy.ViewModels.ImpactItem;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Academy.Services
{
    public class ImpactItemService : IImpactItemService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;
        public ImpactItemService(AppDbContext context,IMapper mapper,IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }

        public async Task CreateAsync(ImpactItemCreateVM model)
        {
            string folderPath = Path.Combine(_env.WebRootPath, "uploads", "impactitem");

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

            var data = _mapper.Map<ImpactItem>(model);

            data.Image = fileName;
            data.CreatedAt = DateTime.Now;

       
            data.ImpactSectionId = model.ImpactSectionId;

            await _context.ImpactItems.AddAsync(data);
            await _context.SaveChangesAsync();
        }

        public  async Task DeleteAsync(int id)
        {
           var data = await _context.ImpactItems.FirstOrDefaultAsync(x => x.Id == id);
            _context.ImpactItems.Remove(data);
            await _context.SaveChangesAsync();

        }

        public async Task<IEnumerable<ImpactItemVM>> GetAllAsync()
        {
            var data = await _context.ImpactItems
                .Include(x => x.ImpactSection) 
                .ToListAsync();

            var result = _mapper.Map<IEnumerable<ImpactItemVM>>(data);

            return result;
        }

        public  async Task<ImpactItem> GetByIdAsync(int id)
        {
           var data = await _context.ImpactItems.FindAsync(id);
            return data;
        }


        public async Task UpdateAsync(ImpactItemEditVM model)
        {
            var data = await _context.ImpactItems
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (data == null) return;

            _mapper.Map(model, data);

            _context.ImpactItems.Update(data);
            await _context.SaveChangesAsync();
        }
    }
}
