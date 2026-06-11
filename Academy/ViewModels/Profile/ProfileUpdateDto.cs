using System.ComponentModel.DataAnnotations;

namespace Academy.ViewModels.Profile
{
    public class ProfileUpdateDto
    {
        [Required(ErrorMessage = "Ad mütləqdir.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Ad 2-50 simvol arasında olmalıdır.")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Soyad mütləqdir.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Soyad 2-50 simvol arasında olmalıdır.")]
        public string LastName { get; set; } = null!;

        [Phone(ErrorMessage = "Telefon nömrəsi formatı düzgün deyil.")]
        [StringLength(20, ErrorMessage = "Telefon nömrəsi maksimum 20 simvol ola bilər.")]
        public string? Phone { get; set; }

        [StringLength(300, ErrorMessage = "Bio maksimum 300 simvol ola bilər.")]
        public string? Bio { get; set; }

        public IFormFile? ProfilePicture { get; set; }
    }
}
