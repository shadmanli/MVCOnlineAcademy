using System.ComponentModel.DataAnnotations;

namespace Academy.ViewModels.Language
{
    public class LanguageCreateVM
    {
        [Required(ErrorMessage = "Dil adı mütləqdir.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Dil adı 2-50 simvol arasında olmalıdır.")]
        public string Name { get; set; } = null!;
    }
}
