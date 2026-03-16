using Academy.Models;
using Academy.ViewModels.AboutUs;
using Academy.ViewModels.FeatureVM;
using Academy.ViewModels.Slider;
using Academy.ViewModels.Statistic;
using AutoMapper;

namespace Academy.Helpers
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<SliderCreateVM, Slider>();
            CreateMap<Slider, SliderVM>();
            CreateMap<Slider, SliderDetailVM>();

            CreateMap<AboutUs, AboutUsVM>();    
            CreateMap<AboutUsCreateVM, AboutUs>();
            CreateMap<AboutUs, AboutUsDetailVM>();


            
             CreateMap<StatisticCreateVM, Statistic>();
            CreateMap<Statistic, StatisticVM>();
            CreateMap<Statistic, StatisticDetailVM>();
          
             CreateMap<Feature, FeatureVM>();
        

        }
    }
}
