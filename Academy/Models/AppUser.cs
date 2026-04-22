using Microsoft.AspNetCore.Identity;

namespace Academy.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
