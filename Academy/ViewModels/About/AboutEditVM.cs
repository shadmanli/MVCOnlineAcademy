namespace Academy.ViewModels.About
{
    public class AboutEditVM
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public string Image { get; set; }   
        public string Video { get; set; }

        public IFormFile? NewImage { get; set; }
        public IFormFile? NewVideo { get; set; }
    }
}
