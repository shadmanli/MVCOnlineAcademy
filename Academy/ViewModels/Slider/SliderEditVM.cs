namespace Academy.ViewModels.Slider
{
    public class SliderEditVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile? Photo { get; set; }
         public string? Image { get; set; }
    }
}
