namespace Academy.Models
{
    public class Article:BaseEntity
    {
        public string Description { get; set; }
        public string Image {  get; set; }
        public string Text { get; set; }
        public int TopicId { get; set; }
        public Topic Topic { get; set; }
    }
}
