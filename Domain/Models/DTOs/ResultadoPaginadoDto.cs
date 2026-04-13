namespace Domain.Models.DTOs
{
    public class ResultadoPaginadoDto<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int TotalPages(int pageSize) => (int)Math.Ceiling((decimal)TotalCount / pageSize);
    }
}
