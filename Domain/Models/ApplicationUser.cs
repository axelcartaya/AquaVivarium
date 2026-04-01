using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(50)]
        public string? Alias { get; set; }
        public virtual ICollection<Acuario> Acuarios { get; set; } = new List<Acuario>();
    }
}
