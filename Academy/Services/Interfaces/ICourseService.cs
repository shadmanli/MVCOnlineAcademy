using Academy.ViewModels.Course;

namespace Academy.Services.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseVM>> GetAllAsync();
        Task CreateAsync(CourseCreateVM model);
        Task<CourseDetailVM> GetByIdAsync(int id);
        Task UpdateAsync(CourseEditVM model); 
        Task DeleteAsync(int id);
    }
}
