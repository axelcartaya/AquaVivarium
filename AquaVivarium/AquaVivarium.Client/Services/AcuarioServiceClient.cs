using Domain.Interfaces.Services;
using Domain.Models;
using System.Net.Http.Json;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace AquaVivarium.Client.Services
{
    public class AcuarioServiceClient : IAcuarioService
    {
        private readonly HttpClient _httpClient;

        public AcuarioServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Acuario?> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Acuario>($"api/Acuario/{id}");
        }

        public async Task<IEnumerable<Acuario>> GetByUsuarioIdAsync(string usuarioId)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Acuario>>($"api/Acuario/usuario/{usuarioId}") ?? new List<Acuario>();
        }

        public async Task<Acuario> AddAsync(Acuario acuario)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Acuario", acuario);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<Acuario>() ?? acuario;
        }

        public async Task UpdateAsync(Acuario acuario)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Acuario/{acuario.Id}", acuario);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Acuario/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
