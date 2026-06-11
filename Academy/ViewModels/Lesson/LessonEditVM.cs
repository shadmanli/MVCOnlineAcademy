using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Academy.ViewModels.Lesson
{
    public class LessonEditVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Dərs başlığı mütləqdir.")]
        [StringLength(200, MinimumLength = 2)]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Kurs seçilməlidir.")]
        [Range(1, int.MaxValue, ErrorMessage = "Kurs seçilməlidir.")]
        public int CourseId { get; set; }

        public List<SelectListItem>? Courses { get; set; }
    }
}
