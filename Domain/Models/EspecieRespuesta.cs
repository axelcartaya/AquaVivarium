using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

public partial class EspecieRespuesta
{
    [Key]
    public int Id { get; set; }

    public int ConsultaId { get; set; }

    [StringLength(450)]
    public string UsuarioId { get; set; } = null!;

    [NotMapped]
    public string? NombreUsuario { get; set; }

    public string Cuerpo { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? FechaPublicacion { get; set; }

    [ForeignKey("ConsultaId")]
    [InverseProperty("EspecieRespuesta")]
    public virtual EspecieConsulta Consulta { get; set; } = null!;
}
