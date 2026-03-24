namespace Academy.Models
{
    public class ImpactItem:BaseEntity
    {
        public string Image { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? ImpactSectionId { get; set; }
         public ImpactSection ImpactSection { get; set; }
    }
}
