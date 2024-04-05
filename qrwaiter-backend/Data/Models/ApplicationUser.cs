using Microsoft.AspNetCore.Identity;

namespace qrwaiter_backend.Data.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
     public Guid IdRestaurant { get; set; } = Guid.NewGuid();
     public Restaurant Restaurant { get; set; } = null!;
    }
}
