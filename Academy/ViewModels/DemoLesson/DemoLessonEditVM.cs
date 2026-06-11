using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Academy.ViewModels.DemoLesson
{
    public class DemoLessonEditVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlıq mütləqdir.")]
        [StringLength(200, MinimumLength = 2)]
        public string Title { get; set; } = null!;

        [StringLength(1000)]
        public string? Description { get; set; }

        public string? ExistingVideoUrl { get; set; }
        public IFormFile? VideoFile { get; set; }

        public string? ExistingThumbnailUrl { get; set; }
        public IFormFile? ThumbnailFile { get; set; }

        [Range(1, 600, ErrorMessage = "Müddət 1-600 dəqiqə arasında olmalıdır.")]
        public int Duration { get; set; }

        [Required(ErrorMessage = "Kurs seçilməlidir.")]
        [Range(1, int.MaxValue, ErrorMessage = "Kurs seçilməlidir.")]
        public int CourseId { get; set; }

        public List<SelectListItem>? Courses { get; set; }
    }
}
