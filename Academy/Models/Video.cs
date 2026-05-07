namespace Academy.Models
{
    public class Video : BaseEntity
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public VideoLevel Level { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
