namespace Academy.Models
{
    public class Instructor : BaseEntity
    {
        public string FullName { get; set; }

        public List<Course> Courses { get; set; }
    }
}
