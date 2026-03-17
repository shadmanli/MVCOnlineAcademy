using Academy.ViewModels.Banner;

namespace Academy.Services.Interfaces
{
    public interface IBannerService
    {
        public Task<IEnumerable<BannerVM>> GetAllAsync();
        public Task CreateAsync(BannerCreateVM model);

        public Task<BannerDetailVM> GetByIdAsync(int id);
        public Task DeleteAsync(int id);

    }
}
