using System.ComponentModel.DataAnnotations;

namespace Academy.ViewModels.About
{
    public class AboutCreateVM
    {
        [Required(ErrorMessage = "Başlıq mütləqdir.")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Başlıq 2-200 simvol arasında olmalıdır.")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Təsvir mütləqdir.")]
        [StringLength(2000, MinimumLength = 5, ErrorMessage = "Təsvir 5-2000 simvol arasında olmalıdır.")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Şəkil mütləqdir.")]
        public IFormFile Image { get; set; } = null!;

        public IFormFile? Video { get; set; }
    }
}
