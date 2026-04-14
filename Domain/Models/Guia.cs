using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Models
{
    [Table("Guias")]
    public class Guia
    {
        [Key]
        public int Id { get; set; }

        public int CategoriaGuiaId { get; set; }

        [Required]
        [StringLength(100)]
        public string Titulo { get; set; } = null!;

        public string? ContenidoHtml { get; set; }

        public DateTime FechaPublicacion { get; set; } = DateTime.UtcNow;

        public bool Activo { get; set; } = true;

        [ForeignKey("CategoriaGuiaId")]
        [InverseProperty("Guias")]
        public virtual CategoriaGuia Categoria { get; set; } = null!;

        [InverseProperty("Guia")]
        public virtual ICollection<ImagenGuia> Imagenes { get; set; } = new List<ImagenGuia>();
    }
}
