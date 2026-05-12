using System.ComponentModel.DataAnnotations;

namespace Academy.ViewModels.Profile
{
    public class DeleteAccountDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}

