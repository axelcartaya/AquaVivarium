using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models.DTOs
{
    public class AcuarioTransferDto
    {
        public string Nombre { get; set; } = "Mi Acuario";
        public int Litros { get; set; }
        public int? LargoCm { get; set; }
        public int? AnchoCm { get; set; }
        public int? AltoCm { get; set; }
        public decimal? PhActual { get; set; }
        public int? TempActual { get; set; }
        public int? GhActual { get; set; }
        public string? TipoSustrato { get; set; }
        public List<EspecieTransferDto> Especies { get; set; } = new();
        public string? NivelIluminacion { get; set; }
        public string? FlujoAgua { get; set; }
        public string? UltimoAnalisisIA { get; set; }
        public bool? TieneCo2 { get; set; }
    }
    public class EspecieTransferDto
    {
        public int EspecieId { get; set; }
        public string Nombre { get; set; } = "";
        public string? TipoEspecie { get; set; }
        public int Cantidad { get; set; }
    }
}
