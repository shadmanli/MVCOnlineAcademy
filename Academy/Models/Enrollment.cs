namespace Academy.Models
{
    public class Enrollment : BaseEntity
    {
        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public DateTime EnrollDate { get; set; }
        public decimal Price { get; set; } 
        public bool IsCompleted { get; set; }
    }
}
