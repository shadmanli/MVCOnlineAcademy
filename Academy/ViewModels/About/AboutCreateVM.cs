namespace Academy.ViewModels.About
{
    public class AboutCreateVM
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public IFormFile Video { get; set; }


    }
}
