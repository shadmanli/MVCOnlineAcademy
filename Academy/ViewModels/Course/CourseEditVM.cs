using Microsoft.AspNetCore.Mvc.Rendering;

namespace Academy.ViewModels.Course
{
    public class CourseEditVM
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; } 
        public IFormFile ImageFile { get; set; } 

        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public int Duration { get; set; }
        public int StudentCount { get; set; }

        public int LanguageId { get; set; }
        public int CategoryId { get; set; }
        public int InstructorId { get; set; }

        public List<Academy.Models.Video>? ExistingVideos { get; set; }
        public List<string>? VideoTitles { get; set; }
        public List<IFormFile>? VideoFiles { get; set; }
        public List<Academy.Models.VideoLevel>? VideoLevels { get; set; }

        public List<SelectListItem> Languages { get; set; }
        public List<SelectListItem> Categories { get; set; }
        public List<SelectListItem> Instructors { get; set; }
    }
}
