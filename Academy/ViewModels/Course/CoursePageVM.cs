using Academy.ViewModels.Category;
using Academy.ViewModels.Instructor;

namespace Academy.ViewModels.Course
{
    public class CoursePageVM
    {
        public List<CourseVM> Courses { get; set; }
        public List<CategoryVM> Categories { get; set; }
        public List<InstructorVM> Instructors { get; set; }

        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? CategoryId { get; set; }
        public int? InstructorId { get; set; }
        public string? Level { get; set; }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string? Search { get; set; }
        public string? Sort { get; set; }
    }
}
