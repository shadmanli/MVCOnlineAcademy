using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Academy.ViewModels.Course
{
    public class CourseCreateVM
    {
        [Required(ErrorMessage = "Başlıq mütləqdir.")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Başlıq 3-200 simvol arasında olmalıdır.")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Təsvir mütləqdir.")]
        [StringLength(5000, MinimumLength = 10)]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Qiymət mütləqdir.")]
        [Range(0, 10000, ErrorMessage = "Qiymət 0 ilə 10000 arasında olmalıdır.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Şəkil mütləqdir.")]
        public IFormFile Image { get; set; } = null!;

        [Required(ErrorMessage = "Dil seçilməlidir.")]
        [Range(1, int.MaxValue, ErrorMessage = "Dil seçilməlidir.")]
        public int LanguageId { get; set; }

        [Required(ErrorMessage = "Kateqoriya seçilməlidir.")]
        [Range(1, int.MaxValue, ErrorMessage = "Kateqoriya seçilməlidir.")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Müəllim seçilməlidir.")]
        [Range(1, int.MaxValue, ErrorMessage = "Müəllim seçilməlidir.")]
        public int InstructorId { get; set; }

        [Range(1, 10000, ErrorMessage = "Müddət 1-10000 dəqiqə arasında olmalıdır.")]
        public int Duration { get; set; }

        public string Level { get; set; } = "Beginner";

        public List<string>? VideoTitles { get; set; }
        public List<IFormFile>? VideoFiles { get; set; }
        public List<Academy.Models.VideoLevel>? VideoLevels { get; set; }

        public List<SelectListItem>? Languages { get; set; }
        public List<SelectListItem>? Categories { get; set; }
        public List<SelectListItem>? Instructors { get; set; }
    }
}
