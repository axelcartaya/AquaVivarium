using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

public partial class Planta
{
    [Key]
    public int EspecieId { get; set; }

    [StringLength(20)]
    public string? Iluminacion { get; set; }

    public bool? NecesitaCo2 { get; set; }

    [StringLength(50)]
    public string? PlanoAcuario { get; set; }

    [ForeignKey("EspecieId")]
    [InverseProperty("Planta")]
    public virtual Especie Especie { get; set; } = null!;
}
