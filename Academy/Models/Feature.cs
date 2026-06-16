namespace Academy.Models
{
    public class Feature : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string? Image { get; set; }
    }
}
