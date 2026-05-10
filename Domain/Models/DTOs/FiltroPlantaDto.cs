using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models.DTOs
{
    public class FiltroPlantaDto : FiltroEspecieDto
    {
        public string? Iluminacion { get; set; }
        public bool? NecesitaCo2 { get; set; }
        public string? Altura { get; set; }
        public string? Crecimiento { get; set; }
    }
}
