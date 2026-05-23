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
    public class AcuarioController : ControllerBase
    {
        private readonly IAcuarioService _acuarioService;
        private readonly IAcuarioIAService _acuarioIAService;

        public AcuarioController(IAcuarioService acuarioService, IAcuarioIAService acuarioIAService)
        {
            _acuarioService = acuarioService;
            _acuarioIAService = acuarioIAService;
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<Acuario>>> GetByUsuarioId(string usuarioId)
        {
            try
            {
                var acuarios = await _acuarioService.GetByUsuarioIdAsync(usuarioId);
                return Ok(acuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Acuario>> GetById(int id)
        {
            try
            {
                var acuario = await _acuarioService.GetByIdAsync(id);
                if (acuario == null) return NotFound($"No se encontró el acuario con ID {id}");

                return Ok(acuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _acuarioService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPost("analizar-ia")]
        public async Task<ActionResult<string>> GenerarAnalisisIA([FromBody] AcuarioTransferDto acuario)
        {
            try
            {
                var resultado = await _acuarioIAService.GenerarAnalisisIAAsync(acuario);
                return Ok(new { Analisis = resultado });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error en el análisis: " + ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Acuario>> GuardarAcuario([FromBody] AcuarioTransferDto dto)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("Usuario no identificado.");
                
                var nuevoAcuario = new Acuario
                {
                    UsuarioId = userId,
                    Nombre = dto.Nombre,
                    Litros = dto.Litros,
                    LargoCm = dto.LargoCm,
                    AnchoCm = dto.AnchoCm,
                    AltoCm = dto.AltoCm,
                    PhActual = dto.PhActual,
                    TempActual = dto.TempActual,
                    GhActual = dto.GhActual,
                    TipoSustrato = dto.TipoSustrato,
                    NivelIluminacion = dto.NivelIluminacion,
                    FlujoAgua = dto.FlujoAgua,
                    UltimoAnalisisIA = dto.UltimoAnalisisIA,
                    TieneCo2 = dto.TieneCo2,
                    AcuarioEspecies = dto.Especies.Select(e => new AcuarioEspecie
                    {
                        EspecieId = e.EspecieId,
                        Cantidad = e.Cantidad
                    }).ToList()
                };

                var acuarioGuardado = await _acuarioService.AddAsync(nuevoAcuario);
                return Ok(acuarioGuardado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al guardar: {ex.Message}");
            }
        }

        [Authorize]
        [HttpGet("mis-acuarios")]
        public async Task<ActionResult<List<Acuario>>> GetMisAcuarios()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();

                var lista = await _acuarioService.GetByUsuarioIdAsync(userId);

                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener acuarios: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarAcuario(int id, [FromBody] AcuarioTransferDto dto)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var acuarioExistente = await _acuarioService.GetByIdAsync(id);

                if (acuarioExistente == null) return NotFound("Acuario no encontrado.");
                if (acuarioExistente.UsuarioId != userId) return Unauthorized("No tienes permiso para editar este acuario.");

                acuarioExistente.Nombre = dto.Nombre;
                acuarioExistente.Litros = dto.Litros;
                acuarioExistente.LargoCm = dto.LargoCm;
                acuarioExistente.AnchoCm = dto.AnchoCm;
                acuarioExistente.AltoCm = dto.AltoCm;
                acuarioExistente.PhActual = dto.PhActual;
                acuarioExistente.TempActual = dto.TempActual;
                acuarioExistente.GhActual = dto.GhActual;
                acuarioExistente.TipoSustrato = dto.TipoSustrato;
                acuarioExistente.NivelIluminacion = dto.NivelIluminacion; 
                acuarioExistente.FlujoAgua = dto.FlujoAgua;
                acuarioExistente.UltimoAnalisisIA = dto.UltimoAnalisisIA;
                acuarioExistente.TieneCo2 = dto.TieneCo2;
                acuarioExistente.AcuarioEspecies.Clear();

                foreach (var e in dto.Especies)
                {
                    acuarioExistente.AcuarioEspecies.Add(new AcuarioEspecie
                    {
                        AcuarioId = id,
                        EspecieId = e.EspecieId,
                        Cantidad = e.Cantidad
                    });
                }

                await _acuarioService.UpdateAsync(acuarioExistente);

                return NoContent(); // 204 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar: {ex.Message}");
            }
        }

    }
}