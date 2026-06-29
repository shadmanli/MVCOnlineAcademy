using Academy.ViewModels.CourseName;

namespace Academy.Services.Interfaces
{
    public interface ICourseNameService
    {
        Task<IEnumerable<CourseNameVM>> GetAllAsync();
        Task<CourseNameVM?> GetByIdAsync(int id);
        Task CreateAsync(CourseNameCreateVM model);
        Task UpdateAsync(int id, CourseNameEditVM model);
        Task DeleteAsync(int id);
        Task<List<CourseNameCategoryDto>> GetCategoriesByNameIdAsync(int courseNameId);
        Task<List<CourseNameVM>> GetActiveNamesAsync();
    }
}
