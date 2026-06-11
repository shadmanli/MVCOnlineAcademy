using System.ComponentModel.DataAnnotations;

namespace Academy.ViewModels.Slider
{
    public class SliderCreateVM
    {
        [Required(ErrorMessage = "Başlıq mütləqdir.")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Başlıq 2-200 simvol arasında olmalıdır.")]
        public string Title { get; set; } = null!;

        [StringLength(500, ErrorMessage = "Təsvir maksimum 500 simvol ola bilər.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Şəkil mütləqdir.")]
        public IFormFile Image { get; set; } = null!;
    }
}
