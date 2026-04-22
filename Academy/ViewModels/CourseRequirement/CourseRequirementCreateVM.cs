using Microsoft.AspNetCore.Mvc.Rendering;

namespace Academy.ViewModels.CourseRequirement
{
    public class CourseRequirementCreateVM
    {
        public string Text { get; set; }
        public int CourseId { get; set; }
        public List<SelectListItem>? Courses { get; set; }
    }
}
