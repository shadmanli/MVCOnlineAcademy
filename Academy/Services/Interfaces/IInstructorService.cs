using Academy.ViewModels.Instructor;

namespace Academy.Services.Interfaces
{
    public interface IInstructorService
    {
        Task<IEnumerable<InstructorVM>> GetAllAsync();
        Task CreateAsync(InstructorCreateVM model);
        Task<InstructorDetailVM> GetByIdAsync(int id);

        Task DeleteAsync(int id);
    }
}
