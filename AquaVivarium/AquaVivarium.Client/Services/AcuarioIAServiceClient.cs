using Domain.Interfaces.Services;
using Domain.Models;
using Domain.Models.DTOs;
using System.Net.Http.Json;
using System.Text.Json;

namespace AquaVivarium.Client.Services
{
    public class AcuarioIAServiceClient : IAcuarioIAService
    {
        private readonly HttpClient _httpClient;

        public AcuarioIAServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GenerarAnalisisIAAsync(AcuarioIADto acuario)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Acuario/analizar-ia", acuario);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<JsonElement>();
                return result.GetProperty("analisis").GetString() ?? "";
            }
            return "Error al contactar con el servidor.";
        }
    }
}
