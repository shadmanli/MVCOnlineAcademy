using System.ComponentModel.DataAnnotations;

namespace Academy.ViewModels.Category
{
    public class CategoryEditVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kateqoriya adı mütləqdir.")]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = null!;
    }
}
