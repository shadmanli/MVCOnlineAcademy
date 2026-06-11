using System.ComponentModel.DataAnnotations;

namespace Academy.ViewModels.Slider
{
    public class SliderEditVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlıq mütləqdir.")]
        [StringLength(200, MinimumLength = 2)]
        public string Title { get; set; } = null!;

        [StringLength(500)]
        public string? Description { get; set; }

        public IFormFile? Photo { get; set; }
        public string? Image { get; set; }
    }
}
