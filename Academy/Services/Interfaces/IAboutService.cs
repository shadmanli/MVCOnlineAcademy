using Academy.Models;
using Academy.ViewModels.About;

namespace Academy.Services.Interfaces
{
    public interface IAboutService
    {
        public Task<IEnumerable<AboutVM>> GetAllAsync();
        public Task CreateAsync(AboutCreateVM about);
        public Task<AboutDetailVM> GetByIdAsync(int id);
        public Task DeleteAsync(int id);
         public  Task EditAsync(AboutEditVM model);

        Task<About> GetEntityByIdAsync(int id);


    }
}
