using Microsoft.AspNetCore.Identity;

namespace Academy.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }

        public string? ProfilePicture { get; set; }
        public string? Bio { get; set; }

        // Notifications
        public bool NotifyNewLesson { get; set; } = true;
        public bool NotifyDiscounts { get; set; } = true;
        public bool NotifyCertificate { get; set; } = true;

        // Assessment Properties
        public ICollection<UserAssessmentResult> AssessmentResults { get; set; } = new List<UserAssessmentResult>();
    }
}
