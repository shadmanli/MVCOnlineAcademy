using Microsoft.AspNetCore.Mvc.Rendering;

namespace Academy.ViewModels.Course
{
    public class CourseCreateVM
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public IFormFile Image { get; set; }

        public int LanguageId { get; set; }
        public int CategoryId { get; set; }
        public int InstructorId { get; set; }

        public int Duration { get; set; }

        public List<string>? VideoTitles { get; set; }
        public List<IFormFile>? VideoFiles { get; set; }
        public List<Academy.Models.VideoLevel>? VideoLevels { get; set; }

        public List<SelectListItem>? Languages { get; set; }
        public List<SelectListItem>? Categories { get; set; }
        public List<SelectListItem>? Instructors { get; set; }
    }
}
