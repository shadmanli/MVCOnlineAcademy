using Academy.ViewModels.CourseRequirement;

namespace Academy.Services.Interfaces
{
    public interface ICourseRequirementService
    {
        Task<IEnumerable<CourseRequirementVM>> GetAllAsync();

        Task CreateAsync(CourseRequirementCreateVM model);

        Task DeleteAsync(int id);
        Task<CourseRequirementDetailVM> GetByIdAsync(int id);
        Task<CourseRequirementEditVM> GetEditAsync(int id);

        Task EditAsync(CourseRequirementEditVM model);
    }
}
