using Academy.ViewModels.Lesson;

namespace Academy.Services.Interfaces
{
    public interface ILessonService
    {
        Task<IEnumerable<LessonVM>> GetAllAsync();
        Task CreateAsync(LessonCreateVM model);
        Task<LessonDetailVM> GetByIdAsync(int id);

        Task DeleteAsync(int id);

        Task<LessonEditVM> GetEditByIdAsync(int id);
        Task EditAsync(LessonEditVM model);
    }
}
