namespace Domain.Models.DTOs
{
    public class FiltroPezDto : FiltroEspecieDto
    {
        public int? TamanoMaximoCm { get; set; }
        public string? Temperamento { get; set; }
        public string? ZonaNado { get; set; }
        public int? GregarismoMinimo { get; set; }
        public string? Alimentacion { get; set; }
    }
}
