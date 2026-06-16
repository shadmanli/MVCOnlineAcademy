using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Academy.ViewModels.LiveClass
{
    public class LiveClassCreateVM
    {
        [Required(ErrorMessage = "Kurs seçilməlidir.")]
        [Range(1, int.MaxValue, ErrorMessage = "Kurs seçilməlidir.")]
        public int CourseId { get; set; }

        public string? TeacherId { get; set; } // Added for Admin/SuperAdmin to pick a teacher

        [Required(ErrorMessage = "Başlıq mütləqdir.")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Başlıq 2-200 simvol arasında olmalıdır.")]
        public string Title { get; set; } = null!;

        [StringLength(500, ErrorMessage = "Mövzu maksimum 500 simvol ola bilər.")]
        public string? Topic { get; set; }

        [Required(ErrorMessage = "Tarix və vaxt mütləqdir.")]
        [DataType(DataType.DateTime, ErrorMessage = "Tarix formatı düzgün deyil.")]
        public DateTime ScheduledDate { get; set; }

        [Required(ErrorMessage = "Müddət mütləqdir.")]
        [Range(15, 480, ErrorMessage = "Müddət 15-480 dəqiqə arasında olmalıdır.")]
        public int DurationMinutes { get; set; }

        [ValidateNever]
        public List<SelectListItem>? Courses { get; set; }

        [ValidateNever]
        public List<SelectListItem>? Teachers { get; set; }

        public int? LessonId { get; set; }

        [ValidateNever]
        public List<SelectListItem>? Lessons { get; set; }
    }
}
