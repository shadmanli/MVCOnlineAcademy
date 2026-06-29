namespace Academy.Models
{
    // Many-to-Many join table: CourseName <-> Category
    public class CourseNameCategory
    {
        public int CourseNameId { get; set; }
        public CourseName CourseName { get; set; } = null!;

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
    }
}
