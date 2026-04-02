using Academy.Models;
using Academy.ViewModels.Course;

namespace Academy.ViewModels.Language
{
    public class LanguageDetailVM
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<CourseVM> Courses { get; set; }
    }
}
