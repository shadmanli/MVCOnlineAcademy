using Academy.Models;
using Academy.ViewModels.ContactItem;

namespace Academy.Services.Interfaces
{
    public interface IContactItemService
    {

        Task<IEnumerable<ContactItemVM>> GetAllAsync();
        Task CreateAsync(ContactItemCreateVM model);
        Task<ContactItem> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
