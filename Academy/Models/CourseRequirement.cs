namespace Academy.Models
{
    public class CourseRequirement:BaseEntity
    {
        public string Text { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
