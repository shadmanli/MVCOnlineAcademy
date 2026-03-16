using Academy.Models;
using Academy.ViewModels.Slider;

namespace Academy.Services.Interfaces
{
    public interface ISliderService
    {
        Task CreateAsync(SliderCreateVM slider);
        Task<IEnumerable<Slider>> GetAllAsync();
        Task<Slider> GetByIdAsync(int id);  
        Task DeleteAsync(Slider slider);
        Task EditAsync(SliderEditVM slider);


    }
}
