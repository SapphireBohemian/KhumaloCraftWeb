using Microsoft.AspNetCore.Identity;

namespace KhumaloCraftWeb.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Navigation property for shopping cart
        public ShoppingCart ShoppingCart { get; set; }
        public string? FcmToken { get; set; }
    }
}
