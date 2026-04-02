using Academy.Models;
using Academy.ViewModels.Mission;

namespace Academy.Services.Interfaces
{
    public interface IMissionService
    {
        public Task<IEnumerable<MissionVM>> GetAllAsync();
        public Task CreateAsync(MissionCreateVM mission);
        public Task<Mission> GetByIdAsync(int id);
        public Task DeleteAsync(Mission mission);
        Task UpdateAsync(MissionEditVM mission);
    }
}
