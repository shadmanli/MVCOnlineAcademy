namespace Academy.Models
{
    public class CourseName : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;

        // Many-to-Many: bir CourseName bir neçə Category-yə aid ola bilər
        public List<CourseNameCategory> CourseNameCategories { get; set; } = new();

        // Bir CourseName ilə yaradılmış kurslar
        public List<Course> Courses { get; set; } = new();
    }
}
