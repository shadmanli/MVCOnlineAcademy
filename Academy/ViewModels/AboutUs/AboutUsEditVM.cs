using System.ComponentModel.DataAnnotations;

namespace Academy.ViewModels.AboutUs
{
    public class AboutUsEditVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlıq mütləqdir.")]
        [StringLength(200, MinimumLength = 2)]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Təsvir mütləqdir.")]
        [StringLength(2000, MinimumLength = 5)]
        public string Description { get; set; } = null!;

        [Phone(ErrorMessage = "Telefon nömrəsi formatı düzgün deyil.")]
        [StringLength(20)]
        public string? Phone { get; set; }

        public string? Image { get; set; }
        public IFormFile? NewImage { get; set; }
    }
}
