using Microsoft.AspNetCore.Identity;

namespace Blog.Models
{
    public class User : IdentityUser
    {
        public DateTime? BirthDate { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? Bio { get; set; }
    }
}
