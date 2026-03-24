using Academy.Models;
using Academy.ViewModels.ImpactSection;

namespace Academy.Services.Interfaces
{
    public interface IImpactSectionService
    {
     public   Task<IEnumerable<ImpactSectionVM>> GetAllAsync();
     public  Task CreateAsync(ImpactSectionCreateVM model);
        public Task<ImpactSectionDetailVM> GetByIdAsync(int id);
        public Task DeleteAsync(int id);

    }
}
