namespace Academy.ViewModels.AboutUs
{
    public class AboutUsEditVM
    {
        public int Id { get; set; }

        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Phone { get; set; }

        public string? Image { get; set; }

        public IFormFile? NewImage { get; set; } 
    }
}
