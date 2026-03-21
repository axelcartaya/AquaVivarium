using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Acuario> Acuarios { get; set; } = new List<Acuario>();
    }
}
