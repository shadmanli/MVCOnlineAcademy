using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Academy.ViewModels.LiveClass
{
    public class LiveClassCreateVM
    {
        [Required(ErrorMessage = "Z?hm?t olmasa kursu seçin.")]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Z?hm?t olmasa mü?llimi seçin.")]
        public int InstructorId { get; set; }

        [Required(ErrorMessage = "Ba?l?q daxil edilm?lidir.")]
        public string Title { get; set; }

        public string Topic { get; set; }

        [Required(ErrorMessage = "Tarix v? vaxt daxil edilm?lidir.")]
        public DateTime ScheduledDate { get; set; }

        [Required(ErrorMessage = "Müdd?t (d?qiq? il?) daxil edilm?lidir.")]
        public int DurationMinutes { get; set; }

        [ValidateNever]
        public List<SelectListItem> Courses { get; set; }
        
        [ValidateNever]
        public List<SelectListItem> Instructors { get; set; }
    }
}
