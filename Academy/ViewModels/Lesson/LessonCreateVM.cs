using Microsoft.AspNetCore.Mvc.Rendering;

namespace Academy.ViewModels.Lesson
{
    public class LessonCreateVM
    {
        public string Title { get; set; }

        public int CourseId { get; set; }
        public List<SelectListItem>? Courses { get; set; }
    }
}
