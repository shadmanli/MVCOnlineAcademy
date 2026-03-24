using Academy.ViewModels.Topic;

namespace Academy.Services.Interfaces
{
    public interface ITopicService
    {
        Task<IEnumerable<TopicVM>> GetAllAsync();
        Task CreateAsync(TopicCreateVM model);
        Task<TopicDetailVM> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
