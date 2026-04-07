using Domain.Interfaces.Services;
using Domain.Models;
using Domain.Models.DTOs;
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

        public async Task<IEnumerable<EspecieBusquedaDto>> BuscarEspeciesAsync(string nombreEspecie)
        {
            if (string.IsNullOrWhiteSpace(nombreEspecie)) return new List<EspecieBusquedaDto>();
            try
            {
                var resultados = await _http.GetFromJsonAsync<List<EspecieBusquedaDto>>($"api/especie/buscar?nombreEspecie={Uri.EscapeDataString(nombreEspecie)}");
                return resultados ?? new List<EspecieBusquedaDto>();
            }
            catch
            {
                return new List<EspecieBusquedaDto>();
            }
        }
    }
}

