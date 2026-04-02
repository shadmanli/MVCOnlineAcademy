namespace Academy.Models
{
    public class Review : BaseEntity
    {
        public int Rating { get; set; }
        public string Comment { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }
    }
}
