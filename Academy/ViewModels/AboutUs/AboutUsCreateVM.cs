using System.ComponentModel.DataAnnotations;

namespace Academy.ViewModels.AboutUs
{
    public class AboutUsCreateVM
    {
        [Required(ErrorMessage = "Başlıq mütləqdir.")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Başlıq 2-200 simvol arasında olmalıdır.")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Təsvir mütləqdir.")]
        [StringLength(2000, MinimumLength = 5, ErrorMessage = "Təsvir 5-2000 simvol arasında olmalıdır.")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Şəkil mütləqdir.")]
        public IFormFile Image { get; set; } = null!;

        [Phone(ErrorMessage = "Telefon nömrəsi formatı düzgün deyil.")]
        [StringLength(20, ErrorMessage = "Telefon nömrəsi maksimum 20 simvol ola bilər.")]
        public string? Phone { get; set; }
    }
}
