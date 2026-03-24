using Academy.ViewModels.Article;

namespace Academy.ViewModels.Topic
{
    public class TopicDetailVM
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string SubTitle { get; set; }

        public IEnumerable<ArticleVM> Articles { get; set; }
    }
}
