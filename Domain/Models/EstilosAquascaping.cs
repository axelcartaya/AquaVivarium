using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

[Table("EstilosAquascaping")]
public partial class EstilosAquascaping
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    [InverseProperty("Estilo")]
    public virtual ICollection<Acuario> Acuarios { get; set; } = new List<Acuario>();

    [InverseProperty("Estilo")]
    public virtual ICollection<EstilosAquascapingImagen> EstilosAquascapingImagenes { get; set; } = new List<EstilosAquascapingImagen>();
}
