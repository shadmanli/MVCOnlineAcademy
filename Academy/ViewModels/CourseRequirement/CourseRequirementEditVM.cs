using Microsoft.AspNetCore.Mvc.Rendering;

namespace Academy.ViewModels.CourseRequirement
{
    public class CourseRequirementEditVM
    {
        public int Id { get; set; }
        public string Text { get; set; }

        public int CourseId { get; set; }
        public List<SelectListItem>? Courses { get; set; }
    }
}
