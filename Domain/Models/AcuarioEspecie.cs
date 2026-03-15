using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

[PrimaryKey("AcuarioId", "EspecieId")]
public partial class AcuarioEspecie
{
    [Key]
    public int AcuarioId { get; set; }

    [Key]
    public int EspecieId { get; set; }

    public int? Cantidad { get; set; }

    [ForeignKey("AcuarioId")]
    [InverseProperty("AcuarioEspecies")]
    public virtual Acuario Acuario { get; set; } = null!;

    [ForeignKey("EspecieId")]
    [InverseProperty("AcuarioEspecies")]
    public virtual Especie Especie { get; set; } = null!;
}
