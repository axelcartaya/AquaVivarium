using Domain.Interfaces.Services;
using Domain.Models;
using Domain.Models.DTOs;
using System.Net.Http.Json;

namespace AquaVivarium.Client.Services
{
    public class PlantaServiceClient : IPlantaService
    {
        private readonly HttpClient _http;

        public PlantaServiceClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<IEnumerable<Planta>> GetAllPlantasAsync()
        {
            return await _http.GetFromJsonAsync<IEnumerable<Planta>>("api/plantas")
                   ?? Enumerable.Empty<Planta>();
        }

        public async Task<Planta?> GetPlantaByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<Planta>($"api/plantas/{id}");
        }

        public async Task<(IEnumerable<Planta> Plantas, int Total)> GetPlantasPaginadasAsync(int page, int pageSize)
        {
            var url = $"api/plantas/paginadas?page={page}&pageSize={pageSize}";
            var resultado = await _http.GetFromJsonAsync<ResultadoPaginadoDto<Planta>>(url);

            if (resultado == null) return (Enumerable.Empty<Planta>(), 0);
            return (resultado.Items, resultado.TotalCount);
        }

        public async Task<(IEnumerable<Planta> Plantas, int TotalCount)> GetPlantasFiltradasAsync(FiltroPlantaDto filtro, int page, int pageSize)
        {
            var response = await _http.PostAsJsonAsync($"api/plantas/filtrar?page={page}&pageSize={pageSize}", filtro);

            response.EnsureSuccessStatusCode();

            var resultado = await response.Content.ReadFromJsonAsync<ResultadoPaginadoDto<Planta>>();

            if (resultado == null) return (new List<Planta>(), 0);
            return (resultado.Items, resultado.TotalCount);
        }
    }
}
