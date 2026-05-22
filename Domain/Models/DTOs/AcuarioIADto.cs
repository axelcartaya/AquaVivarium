using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models.DTOs
{
    public class AcuarioIADto
    {
        public int Litros { get; set; }
        public int? LargoCm { get; set; }
        public int? AnchoCm { get; set; }
        public int? AltoCm { get; set; }
        public decimal? PhActual { get; set; }
        public int? TempActual { get; set; }
        public int? GhActual { get; set; }
        public string? TipoSustrato { get; set; }
        public List<EspecieIaDto> Especies { get; set; } = new();
    }
    public class EspecieIaDto
    {
        public string Nombre { get; set; } = "";
        public string? TipoEspecie { get; set; }
        public int Cantidad { get; set; }
    }
}
