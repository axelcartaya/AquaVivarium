using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

public partial class EspecieConsulta
{
    [Key]
    public int Id { get; set; }

    public int EspecieId { get; set; }

    [StringLength(450)]
    public string UsuarioId { get; set; } = null!;

    [StringLength(150)]
    public string? Titulo { get; set; }

    public string Cuerpo { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? FechaPublicacion { get; set; }

    [ForeignKey("EspecieId")]
    [InverseProperty("EspecieConsulta")]
    public virtual Especie Especie { get; set; } = null!;

    [InverseProperty("Consulta")]
    public virtual ICollection<EspecieRespuesta> EspecieRespuesta { get; set; } = new List<EspecieRespuesta>();
}
