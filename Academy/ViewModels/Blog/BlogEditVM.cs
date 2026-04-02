namespace Academy.ViewModels.Blog
{
    public class BlogEditVM
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }

        public IFormFile? Image { get; set; }   
        public string? ExistingImage { get; set; } 
    }
}
