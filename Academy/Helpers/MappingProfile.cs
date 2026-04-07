using Academy.Models;
using Academy.Services.Interfaces;
using Academy.ViewModels.About;
using Academy.ViewModels.AboutUs;
using Academy.ViewModels.Article;
using Academy.ViewModels.Banner;
using Academy.ViewModels.Blog;
using Academy.ViewModels.Category;
using Academy.ViewModels.ContactItem;
using Academy.ViewModels.ContactSection;
using Academy.ViewModels.Course;
using Academy.ViewModels.CourseFeature;
using Academy.ViewModels.CourseRequirement;
using Academy.ViewModels.Feature;
using Academy.ViewModels.FeatureVM;
using Academy.ViewModels.ImpactItem;
using Academy.ViewModels.ImpactSection;
using Academy.ViewModels.Instructor;
using Academy.ViewModels.Language;
using Academy.ViewModels.Lesson;
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

            CreateMap<AboutUs, AboutUsEditVM>().ReverseMap();

            CreateMap<StatisticCreateVM, Statistic>();
            CreateMap<Statistic, StatisticVM>();
            CreateMap<Statistic, StatisticDetailVM>();
            CreateMap<Statistic, StatisticEditVM>().ReverseMap(); 

            CreateMap<Feature, FeatureVM>();
            CreateMap<FeatureCreateVM, Feature>();
                CreateMap<Feature, FeatureDetailVM>();
            CreateMap<Feature, FeatureEditVM>();
            CreateMap<FeatureEditVM, Feature>();
            CreateMap<Banner, BannerVM>();
            CreateMap<BannerCreateVM, Banner>();
            CreateMap<Banner, BannerDetailVM>();

            CreateMap<Banner, BannerEditVM>();
            CreateMap<BannerEditVM, Banner>();
            CreateMap<BannerDetailVM, BannerEditVM>()
       .ForMember(dest => dest.Image, opt => opt.Ignore()) 
       .ForMember(dest => dest.ExistingImage,
                  opt => opt.MapFrom(src => src.Image));

            CreateMap<Mission, MissionVM>();
                CreateMap<MissionCreateVM, Mission>();
           CreateMap<Mission, MissionDetailVM>();
            CreateMap<Mission, MissionEditVM>()
       .ForMember(dest => dest.ExistingImage, opt => opt.MapFrom(src => src.Image))
       .ForMember(dest => dest.Image, opt => opt.Ignore()); 

            CreateMap <BlogCreateVM, Blog>();
             CreateMap<Blog, BlogVM>();
                CreateMap<Blog, BlogDetailVM>();

            CreateMap<Blog, BlogEditVM>();

            CreateMap<BlogDetailVM, BlogEditVM>()
                .ForMember(dest => dest.Image, opt => opt.Ignore()) 
                .ForMember(dest => dest.ExistingImage,
                           opt => opt.MapFrom(src => src.Image));

            CreateMap<About, AboutVM>();

            CreateMap<AboutCreateVM, About>();

            CreateMap<About, AboutEditVM>().ReverseMap();

            CreateMap<About, AboutDetailVM>();

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
            CreateMap<ImpactSection, ImpactSectionEditVM>().ReverseMap();
            CreateMap<ImpactItem, ImpactItemEditVM>()
    .ForMember(dest => dest.ImpactSectionId,
        opt => opt.MapFrom(src => src.ImpactSectionId));

            CreateMap<ImpactItemEditVM, ImpactItem>();

            CreateMap<ImpactSection, ImpactSectionEditVM>();
            CreateMap<ImpactSectionEditVM, ImpactSection>();
            CreateMap<ImpactSectionDetailVM, ImpactSectionEditVM>();
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
            CreateMap<ContactItemEditVM, ContactItem>();

            CreateMap<ContactItem, ContactItemEditVM>()
                .ForMember(dest => dest.Sections, opt => opt.Ignore());

            CreateMap<ContactSection, ContactSectionVM>()
                .ForMember(dest => dest.Items,
                    opt => opt.MapFrom(src => src.ContactItems));

            CreateMap<ContactSectionCreateVM, ContactSection>();
            CreateMap<ContactSectionDetailVM, ContactSectionEditVM>();

            CreateMap<ContactSection, ContactSectionDetailVM>()
                .ForMember(dest => dest.Items,
                    opt => opt.MapFrom(src => src.ContactItems));
            CreateMap<ContactSectionEditVM, ContactSection>();
            CreateMap<Topic, TopicEditVM>().ReverseMap();
            CreateMap<Article, ArticleEditVM>()
                .ForMember(dest => dest.ExistingImage, opt => opt.MapFrom(src => src.Image))
                .ReverseMap();

            CreateMap<Language, LanguageVM>();

            CreateMap<LanguageCreateVM, Language>();

            CreateMap<Language, LanguageDetailVM>()
                .ForMember(dest => dest.Courses,
                    opt => opt.MapFrom(src => src.Courses));

            CreateMap<Course, CourseVM>();


            CreateMap<Lesson, LessonVM>()
    .ForMember(dest => dest.CourseName,
        opt => opt.MapFrom(src => src.Course.Title));

            CreateMap<LessonCreateVM, Lesson>();

            CreateMap<Lesson, LessonDetailVM>()
                .ForMember(dest => dest.CourseName,
                    opt => opt.MapFrom(src => src.Course.Title));
            CreateMap<Instructor, InstructorVM>();

            CreateMap<InstructorCreateVM, Instructor>();

            CreateMap<Instructor, InstructorDetailVM>()
                .ForMember(dest => dest.Courses,
                    opt => opt.MapFrom(src => src.Courses));

            CreateMap<Course, CourseVM>();

            CreateMap<Course, CourseVM>()
    .ForMember(d => d.LanguageName, o => o.MapFrom(s => s.Language.Name))
    .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category.Name))
    .ForMember(d => d.InstructorName, o => o.MapFrom(s => s.Instructor.FullName));

            CreateMap<CourseCreateVM, Course>();

            CreateMap<Course, CourseDetailVM>()
      .ForMember(d => d.LanguageName,
          o => o.MapFrom(s => s.Language.Name))

      .ForMember(d => d.CategoryName,
          o => o.MapFrom(s => s.Category.Name))

      .ForMember(d => d.InstructorName,
          o => o.MapFrom(s => s.Instructor.FullName))

     
      .ForMember(d => d.Features,
          o => o.MapFrom(s => s.Features != null
              ? s.Features.Select(f => f.Text).ToList()
              : new List<string>()))

     
      .ForMember(d => d.StudentCount,
          o => o.MapFrom(s => s.StudentCount))

      .ForMember(d => d.Rating,
          o => o.MapFrom(s => s.Rating));

            CreateMap<Course, CourseEditVM>();

            CreateMap<CourseEditVM, Course>()
                .ForMember(x => x.ImageUrl, opt => opt.Ignore())
                .ForMember(x => x.CreatedDate, opt => opt.Ignore())
                .ForMember(x => x.UpdatedDate, opt => opt.Ignore());

            CreateMap<Category, CategoryVM>();
            CreateMap<CategoryCreateVM, Category>();
            CreateMap<Category, CategoryDetailVM>()
                .ForMember(dest => dest.Courses, opt => opt.MapFrom(src => src.Courses));
            CreateMap<Course, CourseVM>();


            CreateMap<Language, LanguageEditVM>();
            CreateMap<LanguageEditVM, Language>();

            CreateMap<Lesson, LessonEditVM>();
            CreateMap<LessonEditVM, Lesson>();



            CreateMap<Category, CategoryEditVM>();
            CreateMap<CategoryEditVM, Category>();


            CreateMap<Instructor, InstructorEditVM>();
            CreateMap<InstructorEditVM, Instructor>();


            CreateMap<Course, CourseVM>()
    .ForMember(dest => dest.InstructorName,
        opt => opt.MapFrom(src => src.Instructor.FullName))
    .ForMember(dest => dest.CategoryName,
        opt => opt.MapFrom(src => src.Category.Name))
    .ForMember(dest => dest.LanguageName,
        opt => opt.MapFrom(src => src.Language.Name));





            CreateMap<CourseFeatureCreateVM, CourseFeature>();

   
            CreateMap<CourseFeatureEditVM, CourseFeature>();

        
            CreateMap<CourseFeature, CourseFeatureVM>()
                .ForMember(dest => dest.CourseName,
                           opt => opt.MapFrom(src => src.Course.Title));

     
            CreateMap<CourseFeature, CourseFeatureDetailVM>()
                .ForMember(dest => dest.CourseName,
                           opt => opt.MapFrom(src => src.Course.Title));

            CreateMap<CourseRequirementCreateVM, CourseRequirement>();

       
            CreateMap<CourseRequirementEditVM, CourseRequirement>();


            CreateMap<CourseRequirement, CourseRequirementVM>()
                .ForMember(dest => dest.CourseName,
                    opt => opt.MapFrom(src => src.Course.Title));


            CreateMap<CourseRequirement, CourseRequirementDetailVM>()
                .ForMember(dest => dest.CourseName,
                    opt => opt.MapFrom(src => src.Course.Title));

        }

       
    }
}
