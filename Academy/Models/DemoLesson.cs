namespace Academy.Models
{
    public class DemoLesson : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public int Duration { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
