using Academy.ViewModels.Category;

namespace Academy.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryVM>> GetAllAsync();
        Task CreateAsync(CategoryCreateVM model);
        Task<CategoryDetailVM> GetByIdAsync(int id);
        Task DeleteAsync(int id);
     
    }
}
