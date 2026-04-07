using Microsoft.AspNetCore.Mvc.Rendering;

namespace Academy.ViewModels.CourseFeature
{
    public class CourseFeatureCreateVM
    {
        public string Text { get; set; }

        public int CourseId { get; set; }
        public List<SelectListItem>? Courses { get; set; }
    }
}
