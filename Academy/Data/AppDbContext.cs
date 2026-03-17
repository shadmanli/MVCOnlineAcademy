using Academy.Models;
using Microsoft.EntityFrameworkCore;

namespace Academy.Data
{
    public class AppDbContext:DbContext
    {
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<AboutUs> AboutUs { get; set; }
        public DbSet<Statistic> Statistics { get; set; }
        public DbSet<Feature> Feature { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
       
        
    }
}
