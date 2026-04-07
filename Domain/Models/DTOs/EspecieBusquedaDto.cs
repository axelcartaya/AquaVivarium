namespace Domain.Models.DTOs
{
    public class EspecieBusquedaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? NombreCientifico { get; set; }
        public string? TipoEspecie { get; set; }
        public string? ImagenUrl { get; set; }
    }
}
