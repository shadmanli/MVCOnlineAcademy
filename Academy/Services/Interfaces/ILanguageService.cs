using Academy.ViewModels.Language;

namespace Academy.Services.Interfaces
{
    public interface ILanguageService
    {
        Task<IEnumerable<LanguageVM>> GetAllAsync();
        Task CreateAsync(LanguageCreateVM model);
        Task<LanguageDetailVM> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
