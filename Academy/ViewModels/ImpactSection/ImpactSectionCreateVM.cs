using System.ComponentModel.DataAnnotations;

namespace Academy.ViewModels.ImpactSection
{
    public class ImpactSectionCreateVM
    {
        [Required(ErrorMessage = "Başlıq mütləqdir.")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Başlıq 2-200 simvol arasında olmalıdır.")]
        public string Title { get; set; } = null!;

        [StringLength(300, ErrorMessage = "Alt başlıq maksimum 300 simvol ola bilər.")]
        public string? SubTitle { get; set; }
    }
}
