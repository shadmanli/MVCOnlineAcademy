using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Academy.ViewModels.Instructor
{
    public class InstructorEditVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad Soyad mütləqdir.")]
        [StringLength(100, MinimumLength = 2)]
        public string FullName { get; set; } = null!;

        [StringLength(100)]
        public string? Title { get; set; }

        public string? Bio { get; set; }

        public IFormFile? Image { get; set; }
        public string? ExistingImage { get; set; }

        public List<int> CategoryIds { get; set; } = new();
        public List<SelectListItem>? AllCategories { get; set; }
    }
}
