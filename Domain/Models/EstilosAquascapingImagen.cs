using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

public partial class EstilosAquascapingImagen
{
    [Key]
    public int Id { get; set; }

    public int EstiloId { get; set; }

    [StringLength(255)]
    public string Url { get; set; } = null!;

    [StringLength(100)]
    public string? AltText { get; set; }

    [ForeignKey("EstiloId")]
    [InverseProperty("EstilosAquascapingImagenes")]
    public virtual EstilosAquascaping Estilo { get; set; } = null!;
}
