using Domain.Interfaces.Services;
using Domain.Models;
using System.Net.Http.Json;

namespace AquaVivarium.Client.Services
{
    public class EspecieServiceClient(HttpClient http) : IEspecieService
    {
        private readonly HttpClient _http = http;

        public async Task AddConsultaAsync(EspecieConsulta consulta)
        {
            var response = await _http.PostAsJsonAsync($"api/especie/{consulta.EspecieId}/consultas", consulta.Cuerpo);
            response.EnsureSuccessStatusCode();
        }

        public async Task AddRespuestaAsync(EspecieRespuesta respuesta)
        {
            var response = await _http.PostAsJsonAsync($"api/especie/consultas/{respuesta.ConsultaId}/respuestas", respuesta.Cuerpo);
            response.EnsureSuccessStatusCode();
        }
    }
}

