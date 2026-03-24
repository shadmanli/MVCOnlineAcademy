using Microsoft.AspNetCore.Mvc.Rendering;

namespace Academy.ViewModels.Article
{
    public class ArticleCreateVM
    {
        public IFormFile Image { get; set; }  

        public string Description { get; set; }
        public string Text { get; set; }

        public int TopicId { get; set; }

        public List<SelectListItem> Topics { get; set; } 
    }
}
