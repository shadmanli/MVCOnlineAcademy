using Academy.Models;
using Academy.ViewModels.AboutUs;

namespace Academy.Services.Interfaces
{
    public interface IAboutUsService
    {
        Task<IEnumerable<AboutUs>> GetAllAsync();
        Task CreateAsync(AboutUsCreateVM aboutUs);

        Task<AboutUs> GetByIdAsync(int id);

        Task DeleteAsync(AboutUs aboutus);
        Task EditAsync(AboutUsEditVM model);

    }
}
