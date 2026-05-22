using Domain.Interfaces.Services;
using Domain.Models;
using Domain.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AquaVivarium.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EspecieController : ControllerBase
    {
        private readonly IEspecieService _especieService;

        public EspecieController(IEspecieService especieService)
        {
            _especieService = especieService;
        }

        [HttpPost("{especieId}/consultas")]
        [Authorize]
        public async Task<IActionResult> PostConsulta(int especieId, [FromBody] string cuerpo)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var consulta = new EspecieConsulta
            {
                EspecieId = especieId,
                UsuarioId = userId,
                Cuerpo = cuerpo
            };

            await _especieService.AddConsultaAsync(consulta);
            return Ok();
        }

        [HttpPost("consultas/{consultaId}/respuestas")]
        [Authorize]
        public async Task<IActionResult> PostRespuesta(int consultaId, [FromBody] string cuerpo)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var respuesta = new EspecieRespuesta
            {
                ConsultaId = consultaId,
                UsuarioId = userId,
                Cuerpo = cuerpo
            };

            await _especieService.AddRespuestaAsync(respuesta);
            return Ok();
        }

        [HttpGet("buscar")]
        public async Task<ActionResult<IEnumerable<EspecieBusquedaDto>>> Buscar([FromQuery] string? nombreEspecie)
        {
            if (string.IsNullOrWhiteSpace(nombreEspecie))
                return Ok(new List<EspecieBusquedaDto>());

            return Ok(await _especieService.BuscarEspeciesAsync(nombreEspecie));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Especie>> GetById(int id)
        {
            try
            {
                var especie = await _especieService.GetByIdAsync(id);
                if (especie == null) return NotFound();

                return Ok(especie);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }
    }
}
