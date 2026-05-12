using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Academy.ViewModels.Profile
{
    public class ProfileUpdateDto
    {
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }
        
        public string Phone { get; set; }
        
        [MaxLength(300)]
        public string Bio { get; set; }
        
        public IFormFile ProfilePicture { get; set; }
    }
}

