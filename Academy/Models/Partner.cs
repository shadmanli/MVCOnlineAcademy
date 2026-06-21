namespace Academy.Models
{
    public class Partner : BaseEntity
    {
        public string Name { get; set; }
        public string? Image { get; set; }
        public int Order { get; set; } = 0;
        public bool IsActive { get; set; } = true;
    }
}
