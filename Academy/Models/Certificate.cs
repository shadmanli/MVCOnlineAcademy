namespace Academy.Models
{
    public class Certificate : BaseEntity
    {
        public string CertificateNumber { get; set; } = null!; // Unikal ID
        public string AppUserId { get; set; } = null!;
        public AppUser AppUser { get; set; } = null!;
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;
        public DateTime IssuedAt { get; set; } = DateTime.Now;
        public bool IsRevoked { get; set; } = false;
        public DateTime? RevokedAt { get; set; }
        public string? RevokeReason { get; set; }
        public bool IsManual { get; set; } = false; // Admin/müəllim manual vermişsə
    }
}
