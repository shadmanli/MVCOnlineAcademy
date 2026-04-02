using Academy.Models;
using Academy.ViewModels.Feature;
using Academy.ViewModels.FeatureVM;

namespace Academy.Services.Interfaces
{
    public interface IFeatureService
    {
        public Task<IEnumerable<FeatureVM>> GetAllAsync();
        public Task CreateAsync(FeatureCreateVM model);
        public Task<Feature> GetByIdAsync(int id);
        public Task DeleteAsync(Feature feature);
        Task UpdateAsync(FeatureEditVM model);
        Task<FeatureEditVM> GetByIdForEditAsync(int id);
    }
}
