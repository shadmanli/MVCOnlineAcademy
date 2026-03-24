namespace Academy.ViewModels.Article
{
    public class ArticleVM
    {
        public int Id { get; set; }
        public string Image { get; set; }   
        public string Description { get; set; }
        public string Text { get; set; }

        public int TopicId { get; set; }
        public string TopicName { get; set; }
    }
}
