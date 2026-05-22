using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Domain.Interfaces.Services;
using Domain.Models;
using Domain.Models.DTOs;

namespace AquaVivarium.Services
{
    public class AcuarioIAService(HttpClient http, IConfiguration config) : IAcuarioIAService
    {
        private readonly HttpClient _http = http;
        private readonly string _apiKey = config["GeminiApiKey"] ?? throw new ArgumentNullException("GeminiApiKey no configurada");

        public async Task<string> GenerarAnalisisIAAsync(AcuarioIADto acuario)
        {
            var especiesTexto = string.Join(", ", acuario.Especies.Select(e => $"{e.Cantidad}x {e.Nombre} ({e.TipoEspecie})"));

            var prompt = $@"
                Actúa como un ictiólogo y paisajista acuático experto. 
                Analiza la convivencia de este ecosistema:
                - Tanque: {acuario.Litros}L ({acuario.LargoCm}x{acuario.AnchoCm}x{acuario.AltoCm} cm)
                - Sustrato: {acuario.TipoSustrato}
                - Parámetros: pH {acuario.PhActual}, Temp {acuario.TempActual}°C, GH {acuario.GhActual}
                - Especies: {especiesTexto}

                INSTRUCCIONES ESTRICTAS DE FORMATO:
                1. NO uses formato Markdown (está PROHIBIDO usar asteriscos ** o símbolos #).
                2. Usa texto plano y utiliza los emojis indicados como viñetas.
                3. Sé muy directo, profesional y no superes las 150 palabras.
                4. Devuelve el análisis utilizando EXACTAMENTE esta estructura:

                🐟 TEMPERAMENTO: [Analiza si se van a atacar o estresar]
                🌊 ZONAS DE NADO: [Analiza si el fondo o la superficie están colapsados]
                ⚖️ CARGA BIOLÓGICA: [Analiza si hay demasiados peces para esos litros]
                🌱 ENTORNO: [Analiza si el sustrato y el agua son los correctos para las plantas]
                💡 RECOMENDACIÓN: [Da 1 o 2 consejos claros y accionables para solucionar los problemas detectados o mejorar el ecosistema]
                ⚠️ VEREDICTO: [Escribe solo: INVIABLE, REQUIERE CAMBIOS o VIABLE] - [Una frase final de conclusión].";

            var requestBody = new
            {
                contents = new[]
                {
                    new { parts = new[] { new { text = prompt } } }
                }
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            var url = "https://generativelanguage.googleapis.com/v1beta/models/gemini-flash-lite-latest:generateContent";

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);

            requestMessage.Headers.Add("X-goog-api-key", _apiKey.Trim());
            requestMessage.Content = jsonContent;

            var response = await _http.SendAsync(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                var errorGoogle = await response.Content.ReadAsStringAsync();
                return $"Google rechazó la petición ({(int)response.StatusCode}): {errorGoogle}";
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(jsonResponse);

            try
            {
                var textoGenerado = doc.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                return textoGenerado ?? "Análisis no disponible.";
            }
            catch
            {
                return "La Inteligencia Artificial devolvió un formato irreconocible.";
            }
        }
    }
}
