using Academy.ViewModels.ContactSection;

namespace Academy.Services.Interfaces
{
    public interface IContactSectionService
    {
        Task<IEnumerable<ContactSectionVM>> GetAllAsync();
        Task CreateAsync(ContactSectionCreateVM model);
        Task<ContactSectionDetailVM> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task UpdateAsync(ContactSectionEditVM model);
    }
}
