using System.ComponentModel.DataAnnotations;

namespace Academy.ViewModels.Profile
{
    public class ChangePasswordDto
    {
        [Required]
        public string OldPassword { get; set; }
        
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", ErrorMessage = "Sifr? minimum 8 simvol olub, böyük h?rf v? r?q?m daxil etm?lidir.")]
        public string NewPassword { get; set; }
        
        [Required]
        [Compare("NewPassword", ErrorMessage = "Sifr?l?r uygun deyil.")]
        public string ConfirmPassword { get; set; }
    }
}

