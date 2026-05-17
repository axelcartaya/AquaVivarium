using Domain.Interfaces.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace AquaVivarium.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaGuiaController : ControllerBase
    {
        private readonly ICategoriaGuiaService _categoriaGuiaService;

        public CategoriaGuiaController(ICategoriaGuiaService categoriaGuiaService)
        {
            _categoriaGuiaService = categoriaGuiaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaGuia>>> GetAll()
        {
            try
            {
                var estilos = await _categoriaGuiaService.GetAllAsync();
                return Ok(estilos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaGuia>> GetById(int id)
        {
            try
            {
                var estilo = await _categoriaGuiaService.GetByIdAsync(id);
                if (estilo == null) return NotFound($"No se encontró el estilo con ID {id}");

                return Ok(estilo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

    }
}
