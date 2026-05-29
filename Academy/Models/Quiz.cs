using System.Collections.Generic;

namespace Academy.Models
{
    public class Quiz : BaseEntity
    {
        public string Title { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public ICollection<AssessmentQuestion> Questions { get; set; } = new List<AssessmentQuestion>();

        public ICollection<UserAssessmentResult> Results { get; set; } = new List<UserAssessmentResult>();
    }
}
