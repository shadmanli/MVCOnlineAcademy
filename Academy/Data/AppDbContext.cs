using Academy.Models;
using Microsoft.EntityFrameworkCore;

namespace Academy.Data
{
    public class AppDbContext:DbContext
    {
        public DbSet<Slider> Sliders { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
       
        
    }
}
