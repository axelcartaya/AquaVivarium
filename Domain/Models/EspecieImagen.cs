using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

public partial class EspecieImagen
{
    [Key]
    public int Id { get; set; }

    public int EspecieId { get; set; }

    [StringLength(255)]
    public string Url { get; set; } = null!;

    [StringLength(100)]
    public string? AltText { get; set; }

    [StringLength(100)]
    public string? DerechosAutor { get; set; }

    [ForeignKey("EspecieId")]
    [InverseProperty("EspecieImagenes")]
    public virtual Especie Especie { get; set; } = null!;
}
