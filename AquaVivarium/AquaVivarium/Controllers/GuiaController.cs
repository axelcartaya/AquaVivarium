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

        [HttpGet("categoria/{categoriaId}")]
        public async Task<ActionResult<IEnumerable<Guia>>> GetByCategoriaId(int categoriaId)
        {
            var guias = await _guiaService.GetByCategoriaIdAsync(categoriaId);
            return Ok(guias);
        }
    }
}
