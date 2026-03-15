using Academy.Models;
using Academy.ViewModels.Slider;
using AutoMapper;

namespace Academy.Helpers
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<SliderCreateVM, Slider>();
            CreateMap<Slider, SliderVM>();
        }
    }
}
