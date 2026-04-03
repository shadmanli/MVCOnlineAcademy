using Academy.ViewModels.Instructor;

namespace Academy.Services.Interfaces
{
    public interface IInstructorService
    {
        Task<IEnumerable<InstructorVM>> GetAllAsync();
        Task CreateAsync(InstructorCreateVM model);
        Task<InstructorDetailVM> GetByIdAsync(int id);
        Task<InstructorEditVM> GetEditByIdAsync(int id);
        Task EditAsync(InstructorEditVM model);
        Task DeleteAsync(int id);
    }
}
