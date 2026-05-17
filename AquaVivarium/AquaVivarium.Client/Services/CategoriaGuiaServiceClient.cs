using Domain.Interfaces.Services;
using Domain.Models;
using System.Net.Http.Json;

namespace AquaVivarium.Client.Services
{
    public class CategoriaGuiaServiceClient : ICategoriaGuiaService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/CategoriaGuia";

        public CategoriaGuiaServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<CategoriaGuia>> GetAllAsync()
        {
            try
            {
                var resultados = await _httpClient.GetFromJsonAsync<IEnumerable<CategoriaGuia>>(BaseUrl);
                return resultados ?? new List<CategoriaGuia>();
            }
            catch
            {
                return new List<CategoriaGuia>();
            }
        }

        public async Task<CategoriaGuia?> GetByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<CategoriaGuia>($"{BaseUrl}/{id}");
            }
            catch
            {
                return null;
            }
        }

        public async Task<CategoriaGuia> AddAsync(CategoriaGuia estilo)
        {
            var response = await _httpClient.PostAsJsonAsync(BaseUrl, estilo);
            response.EnsureSuccessStatusCode();

            var estiloCreado = await response.Content.ReadFromJsonAsync<CategoriaGuia>();
            return estiloCreado ?? estilo;
        }

        public async Task UpdateAsync(CategoriaGuia estilo)
        {
            var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{estilo.Id}", estilo);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
