using Academy.ViewModels.Course;

namespace Academy.ViewModels.Category
{
    public class CategoryDetailVM
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<CourseVM> Courses { get; set; }
    }
}
