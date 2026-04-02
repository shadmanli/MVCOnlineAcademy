namespace Academy.Models
{
    public class Student:BaseEntity
    {
        public string FullName { get; set; }
        public string Email { get; set; }

        public List<Enrollment> Enrollments { get; set; }
    }
}
