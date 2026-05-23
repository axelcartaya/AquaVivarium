using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

public partial class Acuario
{
    [Key]
    public int Id { get; set; }

    [StringLength(450)]
    public string UsuarioId { get; set; } = null!;

    [StringLength(100)]
    public string Nombre { get; set; } = null!;

    public int Litros { get; set; }

    public int? LargoCm { get; set; }
    public int? AnchoCm { get; set; }
    public int? AltoCm { get; set; }

    [Column(TypeName = "decimal(4, 2)")]
    public decimal? PhActual { get; set; }
    public int? TempActual { get; set; }

    public int? GhActual { get; set; }

    [StringLength(20)]
    public string? NivelIluminacion { get; set; }

    public bool? TieneCo2 { get; set; }

    [StringLength(50)]
    public string? FlujoAgua { get; set; }

    [StringLength(50)]
    public string? TipoSustrato { get; set; }
    public string? UltimoAnalisisIA { get; set; }

    [InverseProperty("Acuario")]
    public virtual ICollection<AcuarioEspecie> AcuarioEspecies { get; set; } = new List<AcuarioEspecie>();

    [ForeignKey("UsuarioId")]
    public virtual ApplicationUser? Usuario { get; set; }
}