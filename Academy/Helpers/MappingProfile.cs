using Academy.Models;
using Academy.Services.Interfaces;
using Academy.ViewModels.About;
using Academy.ViewModels.AboutUs;
using Academy.ViewModels.Article;
using Academy.ViewModels.Banner;
using Academy.ViewModels.Blog;
using Academy.ViewModels.ContactItem;
using Academy.ViewModels.ContactSection;
using Academy.ViewModels.Feature;
using Academy.ViewModels.FeatureVM;
using Academy.ViewModels.ImpactItem;
using Academy.ViewModels.ImpactSection;
using Academy.ViewModels.Mission;
using Academy.ViewModels.Slider;
using Academy.ViewModels.Statistic;
using Academy.ViewModels.Topic;
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
            CreateMap<FeatureCreateVM, Feature>();
                CreateMap<Feature, FeatureDetailVM>();

            CreateMap<Banner, BannerVM>();
             CreateMap<BannerCreateVM, Banner>();

            CreateMap<Banner, BannerDetailVM>();

            CreateMap<Mission, MissionVM>();
                CreateMap<MissionCreateVM, Mission>();
           CreateMap<Mission, MissionDetailVM>();


            CreateMap <BlogCreateVM, Blog>();
             CreateMap<Blog, BlogVM>();
                CreateMap<Blog, BlogDetailVM>();


            CreateMap<About, AboutVM>();
             CreateMap<AboutCreateVM, About>();


                CreateMap<AboutVM, AboutDetailVM>();


            CreateMap<ImpactItem, ImpactItemVM>()
    .ForMember(dest => dest.SectionName,
        opt => opt.MapFrom(src => src.ImpactSection.Title));
            CreateMap<ImpactItemCreateVM, ImpactItem>();
            CreateMap<ImpactItem, ImpactItemDetailVM>();
            CreateMap<ImpactSection, ImpactSectionVM>()
    .ForMember(dest => dest.Items,
        opt => opt.MapFrom(src => src.ImpactItems));

            CreateMap<ImpactSectionCreateVM, ImpactSection>();
            CreateMap<ImpactSection, ImpactSectionDetailVM>()
       .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.ImpactItems));

            CreateMap<ImpactItem, ImpactItemVM>();

            CreateMap<Article, ArticleVM>()
      .ForMember(dest => dest.TopicName,
          opt => opt.MapFrom(src => src.Topic.Title));

            CreateMap<ArticleCreateVM, Article>();
            CreateMap<Article, ArticleDetailVM>();

            CreateMap<Topic, TopicVM>()
                .ForMember(dest => dest.Articles,
                    opt => opt.MapFrom(src => src.Articles));

            CreateMap<TopicCreateVM, Topic>();

            CreateMap<Topic, TopicDetailVM>()
                .ForMember(dest => dest.Articles,
                    opt => opt.MapFrom(src => src.Articles));


            CreateMap<ContactItem, ContactItemVM>()
    .ForMember(dest => dest.SectionName,
        opt => opt.MapFrom(src => src.ContactSection.Title));

            CreateMap<ContactItemCreateVM, ContactItem>();
            CreateMap<ContactItem, ContactItemDetailVM>()
                .ForMember(dest => dest.SectionName,
                    opt => opt.MapFrom(src => src.ContactSection.Title));

            CreateMap<ContactSection, ContactSectionVM>()
                .ForMember(dest => dest.Items,
                    opt => opt.MapFrom(src => src.ContactItems));

            CreateMap<ContactSectionCreateVM, ContactSection>();

            CreateMap<ContactSection, ContactSectionDetailVM>()
                .ForMember(dest => dest.Items,
                    opt => opt.MapFrom(src => src.ContactItems));
        }

       
    }
}
