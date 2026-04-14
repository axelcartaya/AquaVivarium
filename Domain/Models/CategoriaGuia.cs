using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Models
{
    [Table("CategoriasGuia")]
    public class CategoriaGuia
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; } = null!;

        [StringLength(200)]
        public string? DescripcionBreve { get; set; }

        [StringLength(255)]
        public string? ImagenPortadaUrl { get; set; }
        public bool Activo { get; set; } = true;

        [InverseProperty("Categoria")]
        public virtual ICollection<Guia> Guias { get; set; } = new List<Guia>();
    }
}
