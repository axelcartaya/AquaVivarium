using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Models
{
    [Table("ImagenesGuia")]
    public class ImagenGuia
    {
        [Key]
        public int Id { get; set; }

        public int GuiaId { get; set; }

        [Required]
        [StringLength(255)]
        public string Url { get; set; } = null!;

        [StringLength(100)]
        public string? AltText { get; set; }

        [ForeignKey("GuiaId")]
        [InverseProperty("Imagenes")]
        public virtual Guia Guia { get; set; } = null!;
    }
}
