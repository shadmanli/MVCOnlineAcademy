using Academy.ViewModels.Blog;

namespace Academy.Services.Interfaces
{
    public interface IBlogService
    {
        public Task<IEnumerable<BlogVM>> GetAllAsync();
        public Task CreateAsycn(BlogCreateVM model);

    }
}
