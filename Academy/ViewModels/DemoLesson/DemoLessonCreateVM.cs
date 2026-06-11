using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Academy.ViewModels.DemoLesson
{
    public class DemoLessonCreateVM
    {
        [Required(ErrorMessage = "Başlıq mütləqdir.")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Başlıq 2-200 simvol arasında olmalıdır.")]
        public string Title { get; set; } = null!;

        [StringLength(1000, ErrorMessage = "Təsvir maksimum 1000 simvol ola bilər.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Video faylı mütləqdir.")]
        public IFormFile VideoFile { get; set; } = null!;

        [Required(ErrorMessage = "Thumbnail şəkli mütləqdir.")]
        public IFormFile ThumbnailFile { get; set; } = null!;

        [Range(1, 600, ErrorMessage = "Müddət 1-600 dəqiqə arasında olmalıdır.")]
        public int Duration { get; set; }

        [Required(ErrorMessage = "Kurs seçilməlidir.")]
        [Range(1, int.MaxValue, ErrorMessage = "Kurs seçilməlidir.")]
        public int CourseId { get; set; }

        public List<SelectListItem>? Courses { get; set; }
    }
}
