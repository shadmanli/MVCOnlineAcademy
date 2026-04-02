using Academy.Models;
using Academy.ViewModels.Article;

namespace Academy.Services.Interfaces
{
    public interface IArticleService
    {
        Task<IEnumerable<ArticleVM>> GetAllAsync();
        Task CreateAsync(ArticleCreateVM model);
        Task<Article> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task UpdateAsync(ArticleEditVM model);   

    }
}
