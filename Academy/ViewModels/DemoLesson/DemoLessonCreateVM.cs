using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Academy.ViewModels.DemoLesson
{
    public class DemoLessonCreateVM
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public IFormFile VideoFile { get; set; }
        public IFormFile ThumbnailFile { get; set; }

        public int Duration { get; set; }
        public int CourseId { get; set; }

        public List<SelectListItem>? Courses { get; set; }
    }
}