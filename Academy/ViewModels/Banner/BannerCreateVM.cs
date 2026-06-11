using System.ComponentModel.DataAnnotations;

namespace Academy.ViewModels.Banner
{
    public class BannerCreateVM
    {
        [Required(ErrorMessage = "Başlıq mütləqdir.")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Başlıq 2-200 simvol arasında olmalıdır.")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Şəkil mütləqdir.")]
        public IFormFile Image { get; set; } = null!;

        [Required(ErrorMessage = "Səhifə adı mütləqdir.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Səhifə adı 2-100 simvol arasında olmalıdır.")]
        public string Page { get; set; } = null!;
    }
}
