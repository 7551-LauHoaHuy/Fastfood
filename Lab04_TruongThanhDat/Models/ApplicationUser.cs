using Microsoft.AspNetCore.Identity;

namespace Lab04_TruongThanhDat.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }

        public DateTime Age { get; set; }
        
        public string Address { get; set; } 
    }
}
