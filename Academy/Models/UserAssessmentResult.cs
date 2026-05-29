using System;

namespace Academy.Models
{
    public class UserAssessmentResult : BaseEntity
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int? CourseId { get; set; }
        public Course Course { get; set; }
        
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }

        public string Level { get; set; } // Beginner, Intermediate, Advanced
        public int Score { get; set; }
        public int TotalQuestions { get; set; }
        public int XP { get; set; }
        public double Percentage { get; set; }
    }
}