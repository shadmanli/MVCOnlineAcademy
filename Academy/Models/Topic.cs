namespace Academy.Models
{
    public class Topic:BaseEntity
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public IEnumerable<Article> Articles { get; set; }
    }
}
