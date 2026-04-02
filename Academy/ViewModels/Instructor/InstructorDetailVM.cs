using Academy.ViewModels.Course;

namespace Academy.ViewModels.Instructor
{
    public class InstructorDetailVM
    {
        public string FullName { get; set; }

        public List<CourseVM> Courses { get; set; }
    }
}
