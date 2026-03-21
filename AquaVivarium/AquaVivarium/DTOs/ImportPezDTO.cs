namespace AquaVivarium.DTOs
{
    public class ImportPezDto
    {
        public string Nombre { get; set; } = null!;
        public string? NombreCientifico { get; set; }
        public string? Familia { get; set; }
        public string? Genero { get; set; }
        public string? Origen { get; set; }
        public string? Descripcion { get; set; }
        public decimal PhMin { get; set; }
        public decimal PhMax { get; set; }
        public int TempMin { get; set; }
        public int TempMax { get; set; }
        public int GhMin { get; set; }
        public int GhMax { get; set; }
        public string Dificultad { get; set; } = null!;
        public string TipoEspecie { get; set; } = null!;
        public PezDataDto PezInfo { get; set; } = null!;
    }
    public class PezDataDto
    {
        public int TamanoMaxCm { get; set; }
        public string Temperamento { get; set; } = null!;
        public string ZonaNado { get; set; } = null!;
        public int Gregarismo { get; set; }
        public string Alimentacion { get; set; } = null!;

    }
}
