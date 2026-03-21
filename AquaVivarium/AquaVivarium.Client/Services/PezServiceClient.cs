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
    }
}
