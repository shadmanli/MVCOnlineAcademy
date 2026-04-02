namespace Academy.ViewModels.Banner
{
    public class BannerEditVM
    {
        public string Title { get; set; }
        public string Page { get; set; }

        public IFormFile? Image { get; set; }

        public string? ExistingImage { get; set; }
    }
}
