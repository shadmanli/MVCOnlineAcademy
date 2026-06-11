using System.ComponentModel.DataAnnotations;

namespace Academy.ViewModels.Language
{
    public class LanguageEditVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Dil adı mütləqdir.")]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; } = null!;
    }
}
