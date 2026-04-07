using Academy.ViewModels.CourseFeature;

namespace Academy.Services.Interfaces
{
    public interface ICourseFeatureService
    {
        Task<IEnumerable<CourseFeatureVM>> GetAllAsync();
        Task CreateAsync(CourseFeatureCreateVM model);
        Task<CourseFeatureDetailVM> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task<CourseFeatureEditVM> GetEditByIdAsync(int id);
        Task EditAsync(CourseFeatureEditVM model);
    }
}
