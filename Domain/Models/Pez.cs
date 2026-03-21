using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

public partial class Pez
{
    [Key]
    public int EspecieId { get; set; }

    public int? TamanoMaxCm { get; set; }

    [StringLength(50)]
    public string? Temperamento { get; set; }

    [StringLength(50)]
    public string? ZonaNado { get; set; }

    public int? Gregarismo { get; set; }

    [StringLength(30)]
    public string? Alimentacion { get; set; }

    [ForeignKey("EspecieId")]
    [InverseProperty("Pez")]
    public virtual Especie Especie { get; set; } = null!;
}
