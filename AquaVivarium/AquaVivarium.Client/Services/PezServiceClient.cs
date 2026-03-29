using Domain.Interfaces.Services;
using Domain.Models;
using System.Net.Http.Json;

namespace AquaVivarium.Client.Services
{
    public class PezServiceClient : IPezService
    {
        private readonly HttpClient _http;

        public PezServiceClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<IEnumerable<Pez>> GetAllPecesAsync()
        {
            return await _http.GetFromJsonAsync<IEnumerable<Pez>>("api/peces")
                   ?? Enumerable.Empty<Pez>();
        }

        public async Task<Pez?> GetPezByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<Pez>($"api/peces/{id}");
        }
        public async Task<(IEnumerable<Pez> Peces, int Total)> GetPecesPaginadosAsync(int page, int pageSize)
        {
            var url = $"api/peces/paginados?page={page}&tamaño={pageSize}";
            var resultado = await _http.GetFromJsonAsync<PaginacionHelper>(url);
            if (resultado == null) return (Enumerable.Empty<Pez>(), 0);

            return (resultado.Peces, resultado.Total);
        }

        // Clase de apoyo 
        private class PaginacionHelper
        {
            public List<Pez> Peces { get; set; } = new();
            public int Total { get; set; }
        }
    }
}
