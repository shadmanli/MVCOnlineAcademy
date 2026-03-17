namespace Academy.ViewModels.Blog
{
    public class BlogCreateVM
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public string Name { get; set; }
    }
}
