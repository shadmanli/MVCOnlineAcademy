using System.ComponentModel.DataAnnotations;

namespace Academy.ViewModels.Feature
{
    public class FeatureCreateVM
    {
        [Required(ErrorMessage = "Başlıq mütləqdir.")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Başlıq 2-200 simvol arasında olmalıdır.")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Təsvir mütləqdir.")]
        [StringLength(1000, MinimumLength = 5, ErrorMessage = "Təsvir 5-1000 simvol arasında olmalıdır.")]
        public string Description { get; set; } = null!;

        public IFormFile? Image { get; set; }
    }
}
