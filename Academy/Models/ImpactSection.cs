namespace Academy.Models
{
    public class ImpactSection:BaseEntity
    {
        public string Title { get; set; }
        public string  SubTitle { get; set; }
        public IEnumerable<ImpactItem> ImpactItems { get; set; }

    }
}
