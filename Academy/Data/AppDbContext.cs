using Academy.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Academy.Models;

namespace Academy.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Slider> Sliders { get; set; }
        public DbSet<AboutUs> AboutUs { get; set; }
        public DbSet<Statistic> Statistics { get; set; }
        public DbSet<Feature> Feature { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<Mission> Mission { get; set; }
        public DbSet<About> About { get; set; }
        public DbSet<ImpactItem> ImpactItems { get; set; } 
        public DbSet<ImpactSection> ImpactSections { get; set; }
        public DbSet<Blog> Blog { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<ContactSection> Contacts { get; set; }
        public DbSet<ContactItem> ContactItems {  get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Instructor> Instructors { get; set; }  
        public DbSet<Language> Languages { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<CourseRequirement> CourseRequirements { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<CourseFeature> CourseFeatures { get; set; }

        public DbSet<AssessmentQuestion> AssessmentQuestions { get; set; }
        public DbSet<AssessmentOption> AssessmentOptions { get; set; }
        public DbSet<UserAssessmentResult> UserAssessmentResults { get; set; }
    }
}
