using Academy.Data;
using Academy.Models;
using Academy.Services.Interfaces;
using Academy.ViewModels.ContactSection;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Academy.Services
{
    public class ContactSectionService : IContactSectionService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ContactSectionService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateAsync(ContactSectionCreateVM model)
        {
            var data = _mapper.Map<ContactSection>(model);
            await _context.Contacts.AddAsync(data);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _context.Contacts
                .Include(x => x.ContactItems)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (data == null) return;

            if (data.ContactItems != null && data.ContactItems.Any())
            {
                _context.ContactItems.RemoveRange(data.ContactItems);
            }

            _context.Contacts.Remove(data);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ContactSectionVM>> GetAllAsync()
        {
            var data = await _context.Contacts
                .Include(x => x.ContactItems)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ContactSectionVM>>(data);
        }

        public async Task<ContactSectionDetailVM> GetByIdAsync(int id)
        {
            var data = await _context.Contacts
                .Include(x => x.ContactItems)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (data == null) return null;

            return _mapper.Map<ContactSectionDetailVM>(data);
        }

        public async Task UpdateAsync(ContactSectionEditVM model)
        {
            var data = await _context.Contacts.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (data == null) return;

            _mapper.Map(model, data); 
           

            _context.Contacts.Update(data);
            await _context.SaveChangesAsync();
        }
    }
}
