using Academy.Models;
using Academy.ViewModels.Blog;

namespace Academy.Services.Interfaces
{
    public interface IBlogService
    {
        public Task<IEnumerable<BlogVM>> GetAllAsync();
        public Task CreateAsycn(BlogCreateVM model);

        public Task<BlogDetailVM> GetByIdAsync(int id);   
        public Task DeleteAsync(int id);

    }
}
