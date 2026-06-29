using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Academy.ViewModels.Instructor
{
    public class InstructorCreateVM
    {
        [Required(ErrorMessage = "Ad Soyad mütləqdir.")]
        [StringLength(100, MinimumLength = 2)]
        public string FullName { get; set; } = null!;

        [StringLength(100)]
        public string? Title { get; set; }

        public string? Bio { get; set; }

        public IFormFile? Image { get; set; }

        // Müəllimin ixtisaslaşdığı kateqoriyalar
        public List<int> CategoryIds { get; set; } = new();

        // Dropdown üçün
        public List<SelectListItem>? AllCategories { get; set; }
    }
}
