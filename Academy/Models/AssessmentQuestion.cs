namespace Academy.Models
{
    public class AssessmentQuestion : BaseEntity
    {
        public string Text { get; set; }

        // Subject / Category Relation
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int? CourseId { get; set; }
        public Course Course { get; set; }

        public DifficultyLevel Difficulty { get; set; } = DifficultyLevel.Medium;
        public int Points { get; set; } = 10;

        public ICollection<AssessmentOption> Options { get; set; } = new List<AssessmentOption>();
    }

    public enum DifficultyLevel
    {
        Easy = 1,
        Medium = 2,
        Hard = 3
    }
}