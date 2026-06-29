namespace Academy.Models
{
    public class InstructorCategory
    {
        public int InstructorId { get; set; }
        public Instructor Instructor { get; set; } = null!;

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
    }
}
