using System.ComponentModel.DataAnnotations;

namespace Academy.ViewModels.Account
{
    public class LoginVM
    {
        [Required, EmailAddress, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
