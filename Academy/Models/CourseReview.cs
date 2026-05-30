namespace Academy.Models
{
    public enum ReviewStatus
    {
        Pending = 0,    // Gözlənilir
        Approved = 1,   // Təsdiqləndi
        Rejected = 2    // Rədd edildi
    }

    public class CourseReview : BaseEntity
    {
        public string AppUserId { get; set; } = null!;
        public AppUser AppUser { get; set; } = null!;

        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public string Name { get; set; } = null!;
        public string? Photo { get; set; }

        public int Rating { get; set; } // 1-5

        public string Message { get; set; } = null!;

        public ReviewStatus Status { get; set; } = ReviewStatus.Pending;
    }
}
