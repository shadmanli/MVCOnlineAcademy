namespace Academy.Models
{
    public class ContactMessage : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Website { get; set; }
        public string Message { get; set; } = null!;
        public bool IsRead { get; set; } = false;
        public bool IsReplied { get; set; } = false;
        public DateTime? RepliedAt { get; set; }
    }
}
