namespace Academy.ViewModels.Course
{
    public class CourseDetailVM
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public string LanguageName { get; set; }
        public string CategoryName { get; set; }
        public string InstructorName { get; set; }

        public int Duration { get; set; }
    }
}
