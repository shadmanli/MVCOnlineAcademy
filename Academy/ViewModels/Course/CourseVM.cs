namespace Academy.ViewModels.Course
{
    public class CourseVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }

        public string LanguageName { get; set; }
        public string CategoryName { get; set; }
        public string InstructorName { get; set; }
        public int CategoryId { get; set; }      
        public int InstructorId { get; set; }
        
        public string Level { get; set; }
    }
}
