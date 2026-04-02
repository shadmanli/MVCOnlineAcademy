namespace Academy.Models
{
    public class Language : BaseEntity
    {
        public string Name { get; set; }

        public List<Course> Courses { get; set; }
    }
}
