using Academy.Data;
using Academy.Models;
using Academy.Services.Interfaces;
using Academy.ViewModels.ContactItem;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Academy.Services
{
    public class ContactItemService : IContactItemService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ContactItemService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateAsync(ContactItemCreateVM model)
        {
            var data = _mapper.Map<ContactItem>(model);
            data.CreatedAt = DateTime.Now;

            await _context.ContactItems.AddAsync(data);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _context.ContactItems.FirstOrDefaultAsync(x => x.Id == id);
            _context.ContactItems.Remove(data);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ContactItemVM>> GetAllAsync()
        {
            var data = await _context.ContactItems
                .Include(x => x.ContactSection)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ContactItemVM>>(data);
        }

        public async Task<ContactItem> GetByIdAsync(int id)
        {
            return await _context.ContactItems
                .Include(x => x.ContactSection)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
