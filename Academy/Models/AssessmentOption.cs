namespace Academy.Models
{
    public class AssessmentOption : BaseEntity
    {
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
        public int AssessmentQuestionId { get; set; }
        public AssessmentQuestion AssessmentQuestion { get; set; }
    }
}