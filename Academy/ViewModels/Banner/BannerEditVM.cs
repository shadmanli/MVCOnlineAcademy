using System.ComponentModel.DataAnnotations;

namespace Academy.ViewModels.Banner
{
    public class BannerEditVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlıq mütləqdir.")]
        [StringLength(200, MinimumLength = 2)]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Səhifə adı mütləqdir.")]
        [StringLength(100, MinimumLength = 2)]
        public string Page { get; set; } = null!;

        public IFormFile? Image { get; set; }
        public string? ExistingImage { get; set; }
    }
}
