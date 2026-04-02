using Academy.Models;
using Academy.ViewModels.ImpactItem;

namespace Academy.Services.Interfaces
{
    public interface IImpactItemService
    {
        Task<IEnumerable<ImpactItemVM>> GetAllAsync();
        Task CreateAsync(ImpactItemCreateVM model);
        Task<ImpactItem> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task UpdateAsync(ImpactItemEditVM model);

    }
}
