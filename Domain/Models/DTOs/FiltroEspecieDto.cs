namespace Domain.Models.DTOs
{
    public class FiltroEspecieDto
    {
        public decimal? PhDesde { get; set; }
        public decimal? PhHasta { get; set; }
        public int? TempDesde { get; set; }
        public int? TempHasta { get; set; }
        public int? GhDesde { get; set; }
        public int? GhHasta { get; set; }
        public string? Dificultad { get; set; }
        public string? Nombre { get; set; }
        public string? Familia { get; set; }
        public string? Genero { get; set; }
        public string? Origen { get; set; }
    }
}
