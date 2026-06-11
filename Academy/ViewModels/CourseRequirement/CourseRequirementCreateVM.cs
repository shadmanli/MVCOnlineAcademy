using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Academy.ViewModels.CourseRequirement
{
    public class CourseRequirementCreateVM
    {
        [Required(ErrorMessage = "Tələb mətni mütləqdir.")]
        [StringLength(500, MinimumLength = 2, ErrorMessage = "Mətn 2-500 simvol arasında olmalıdır.")]
        public string Text { get; set; } = null!;

        [Required(ErrorMessage = "Kurs seçilməlidir.")]
        [Range(1, int.MaxValue, ErrorMessage = "Kurs seçilməlidir.")]
        public int CourseId { get; set; }

        public List<SelectListItem>? Courses { get; set; }
    }
}
