namespace Academy.Models
{
    public class VideoProgress : BaseEntity
    {
        public string AppUserId { get; set; } = null!;
        public AppUser AppUser { get; set; } = null!;

        public int VideoId { get; set; }
        public Video Video { get; set; } = null!;

        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public bool IsWatched { get; set; } = false;
        public DateTime? WatchedAt { get; set; }
    }
}
