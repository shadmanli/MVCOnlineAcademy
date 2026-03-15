using Academy.Models;
using Academy.ViewModels.Slider;

namespace Academy.Services.Interfaces
{
    public interface ISliderService
    {
        Task CreateAsync(SliderCreateVM slider);
        Task<IEnumerable<Slider>> GetAllAsync();

    }
}
