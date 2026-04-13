using Domain.Interfaces.Services;
using Domain.Models;
using Domain.Models.DTOs;
using System.Net.Http;
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
            var url = $"api/peces/paginados?page={page}&pageSize={pageSize}";
            var resultado = await _http.GetFromJsonAsync<ResultadoPaginadoDto<Pez>>(url);

            if (resultado == null)
                return (Enumerable.Empty<Pez>(), 0);
            return (resultado.Items, resultado.TotalCount);
        }

        public async Task<(IEnumerable<Pez> Peces, int TotalCount)> GetPecesFiltradosAsync(FiltroPezDto filtro, int page, int pageSize)
        {
            var response = await _http.PostAsJsonAsync($"api/peces/filtrar?page={page}&pageSize={pageSize}", filtro);

            response.EnsureSuccessStatusCode();

            var resultado = await response.Content.ReadFromJsonAsync<ResultadoPaginadoDto<Pez>>();

            if (resultado == null) return (new List<Pez>(), 0);

            return (resultado.Items, resultado.TotalCount);
        }

    }
}
