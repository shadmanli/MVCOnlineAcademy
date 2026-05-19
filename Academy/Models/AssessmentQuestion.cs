namespace Academy.Models
{
    public class AssessmentQuestion : BaseEntity
    {
        public string Text { get; set; }

        // Subject / Category Relation
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<AssessmentOption> Options { get; set; } = new List<AssessmentOption>();
    }
}