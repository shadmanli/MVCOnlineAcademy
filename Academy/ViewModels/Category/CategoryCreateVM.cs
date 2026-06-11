using System.ComponentModel.DataAnnotations;

namespace Academy.ViewModels.Category
{
    public class CategoryCreateVM
    {
        [Required(ErrorMessage = "Kateqoriya adı mütləqdir.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Ad 2-100 simvol arasında olmalıdır.")]
        public string Name { get; set; } = null!;
    }
}
