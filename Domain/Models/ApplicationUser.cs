using Microsoft.AspNetCore.Identity;

namespace Domain.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Acuario> Acuarios { get; set; } = new List<Acuario>(); 
    }

}
