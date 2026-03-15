using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

public partial class Especie
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string Nombre { get; set; } = null!;

    [StringLength(100)]
    public string? NombreCientifico { get; set; }

    public string? Descripcion { get; set; }

    [Column(TypeName = "decimal(4, 2)")]
    public decimal? PhMin { get; set; }

    [Column(TypeName = "decimal(4, 2)")]
    public decimal? PhMax { get; set; }

    public int? TempMin { get; set; }

    public int? TempMax { get; set; }

    public int? GhMin { get; set; }

    public int? GhMax { get; set; }

    [StringLength(20)]
    public string? Dificultad { get; set; }

    [StringLength(50)]
    public string? TipoEspecie { get; set; }

    [InverseProperty("Especie")]
    public virtual ICollection<AcuarioEspecie> AcuarioEspecies { get; set; } = new List<AcuarioEspecie>();

    [InverseProperty("Especie")]
    public virtual ICollection<EspecieConsulta> EspecieConsulta { get; set; } = new List<EspecieConsulta>();

    [InverseProperty("Especie")]
    public virtual ICollection<EspecieImagen> EspecieImagenes { get; set; } = new List<EspecieImagen>();

    [InverseProperty("Especie")]
    public virtual Pez? Pece { get; set; }

    [InverseProperty("Especie")]
    public virtual Planta? Planta { get; set; }
}
