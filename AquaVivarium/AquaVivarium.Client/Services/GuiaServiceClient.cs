using Domain.Interfaces.Services;
using Domain.Models;
using System.Net.Http.Json;

namespace AquaVivarium.Client.Services
{
    public class GuiaServiceClient : IGuiaService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/Guia";

        public GuiaServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Guia>> GetAllAsync()
        {
            try
            {
                var resultados = await _httpClient.GetFromJsonAsync<IEnumerable<Guia>>(BaseUrl);
                return resultados ?? new List<Guia>();
            }
            catch
            {
                return new List<Guia>();
            }
        }

        public async Task<Guia?> GetByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<Guia>($"{BaseUrl}/{id}");
            }
            catch
            {
                return null;
            }
        }

        public async Task<Guia> AddAsync(Guia estilo)
        {
            var response = await _httpClient.PostAsJsonAsync(BaseUrl, estilo);
            response.EnsureSuccessStatusCode();

            var estiloCreado = await response.Content.ReadFromJsonAsync<Guia>();
            return estiloCreado ?? estilo;
        }

        public async Task UpdateAsync(Guia estilo)
        {
            var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{estilo.Id}", estilo);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<Guia>> GetByCategoriaIdAsync(int categoriaId)
        {
            var resultados = await _httpClient.GetFromJsonAsync<IEnumerable<Guia>>($"{BaseUrl}/categoria/{categoriaId}");
            return resultados ?? new List<Guia>();
        }
    }
}
