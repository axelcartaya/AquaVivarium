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

    [Column(TypeName = "decimal(4, 2)")]
    public decimal? PhActual { get; set; }

    public int? TempActual { get; set; }

    public int? EstiloId { get; set; }

    [InverseProperty("Acuario")]
    public virtual ICollection<AcuarioEspecie> AcuarioEspecies { get; set; } = new List<AcuarioEspecie>();
}
