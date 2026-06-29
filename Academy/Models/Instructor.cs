namespace Academy.Models
{
    public class Instructor : BaseEntity
    {
        public string FullName { get; set; }
        public string? Title { get; set; }
        public string? Image { get; set; }
        public string? Bio { get; set; }

        public List<Course> Courses { get; set; }

        // Müəllimin ixtisaslaşdığı kateqoriyalar
        public List<InstructorCategory> InstructorCategories { get; set; } = new();
    }
}
