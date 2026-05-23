using Domain.Interfaces.Services;
using Domain.Models;
using Domain.Models.DTOs;
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

        public async Task<Acuario?> AddAsync(Acuario acuario)
        {
            var response = await _httpClient.PostAsJsonAsync("api/acuario", acuario);

            if (response.IsSuccessStatusCode)            
                return await response.Content.ReadFromJsonAsync<Acuario>();
            
            return null;
        }
        public async Task<Acuario?> AddDesdeDtoAsync(AcuarioTransferDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/acuario", dto);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<Acuario>();

            return null;
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
        public async Task<List<Acuario>> GetMisAcuariosAsync()
        {
            var response = await _httpClient.GetAsync("api/acuario/mis-acuarios");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Acuario>>() ?? new List<Acuario>();
            }
            return new List<Acuario>();
        }
        public async Task<bool> UpdateDesdeDtoAsync(int id, AcuarioTransferDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/acuario/{id}", dto);
            return response.IsSuccessStatusCode;
        }

    }
}
