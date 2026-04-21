using Domain.Interfaces.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace AquaVivarium.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuiaController : ControllerBase
    {
        private readonly IGuiaService _guiaService;

        public GuiaController(IGuiaService guiaService)
        {
            _guiaService = guiaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Guia>>> GetAll()
        {
            try
            {
                var estilos = await _guiaService.GetAllAsync();
                return Ok(estilos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Guia>> GetById(int id)
        {
            try
            {
                var estilo = await _guiaService.GetByIdAsync(id);
                if (estilo == null) return NotFound($"No se encontró el estilo con ID {id}");

                return Ok(estilo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Guia>> Create([FromBody] Guia estilo)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var nuevoEstilo = await _guiaService.AddAsync(estilo);
                return CreatedAtAction(nameof(GetById), new { id = nuevoEstilo.Id }, nuevoEstilo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear el estilo: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Guia estilo)
        {
            if (id != estilo.Id) return BadRequest("El ID de la ruta no coincide con el ID del objeto.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var estiloExistente = await _guiaService.GetByIdAsync(id);
                if (estiloExistente == null) return NotFound($"No se encontró el estilo con ID {id}");

                await _guiaService.UpdateAsync(estilo);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar el estilo: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var estiloExistente = await _guiaService.GetByIdAsync(id);
                if (estiloExistente == null) return NotFound($"No se encontró el estilo con ID {id}");

                await _guiaService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar el estilo: {ex.Message}");
            }
        }

        [HttpGet("categoria/{categoriaId}")]
        public async Task<ActionResult<IEnumerable<Guia>>> GetByCategoriaId(int categoriaId)
        {
            var guias = await _guiaService.GetByCategoriaIdAsync(categoriaId);
            return Ok(guias);
        }
    }
}
