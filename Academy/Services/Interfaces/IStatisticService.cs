using Academy.Models;
using Academy.ViewModels.Statistic;

namespace Academy.Services.Interfaces
{
    public interface IStatisticService
    {
        Task CreateAsync(StatisticCreateVM model);

        Task< IEnumerable<StatisticVM>>GetAllAsync();
        Task<Statistic> GetByIdAsync(int id);
        Task UpdateAsync(Statistic statistic); 
        Task DeleteAsync(Statistic statistic);
    }
}
