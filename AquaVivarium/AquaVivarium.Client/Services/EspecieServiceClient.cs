using Domain.Interfaces.Services;
using Domain.Models;
using System.Net.Http.Json;

namespace AquaVivarium.Client.Services
{
    public class EspecieServiceClient(HttpClient http) : IEspecieService
    {
        private readonly HttpClient _http = http;

        public async Task<List<EspecieImagen>> GetImagenesAsync(int especieId)
        {
            try
            {
                var response = await _http.GetAsync($"api/especie/{especieId}/imagenes");

                if (response.IsSuccessStatusCode)
                {
                    var resultado = await response.Content.ReadFromJsonAsync<List<EspecieImagen>>();
                    return resultado ?? new List<EspecieImagen>();
                }

                return new List<EspecieImagen>(); 
            }
            catch
            {
                return new List<EspecieImagen>(); 
            }
        }
    }
}
