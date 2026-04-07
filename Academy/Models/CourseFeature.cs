namespace Academy.Models
{
    public class CourseFeature:BaseEntity
    {
        public int Id { get; set; }
        public string Text { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
