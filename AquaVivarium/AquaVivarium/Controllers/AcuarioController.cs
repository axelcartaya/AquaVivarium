using Domain.Interfaces.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace AquaVivarium.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcuarioController : ControllerBase
    {
        private readonly IAcuarioService _acuarioService;

        public AcuarioController(IAcuarioService acuarioService)
        {
            _acuarioService = acuarioService;
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

        [HttpPost]
        public async Task<ActionResult<Acuario>> Post([FromBody] Acuario acuario)
        {
            try
            {
                var nuevoAcuario = await _acuarioService.AddAsync(acuario);          
                return CreatedAtAction(nameof(GetById), new { id = nuevoAcuario.Id }, nuevoAcuario); // Devuelve un 201
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Acuario acuario)
        {
            if (id != acuario.Id)       
                return BadRequest("El ID de la URL no coincide con el ID del acuario proporcionado.");
            

            try
            {
                await _acuarioService.UpdateAsync(acuario);
                return NoContent(); // Devuelve 204 
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
    }
}