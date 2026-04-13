using Domain.Interfaces.Services;
using Domain.Models;
using Domain.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AquaVivarium.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PecesController : ControllerBase
    {
        private readonly IPezService _pezService;

        public PecesController(IPezService pezService)
        {
            _pezService = pezService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var pez = await _pezService.GetPezByIdAsync(id);

            if (pez == null)
            {
                return NotFound();
            }

            return Ok(pez);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var peces = await _pezService.GetAllPecesAsync();
            return Ok(peces);
        }

        [HttpGet("paginados")]
        public async Task<IActionResult> GetPecesPaginados([FromQuery] int page = 1, [FromQuery] int pageSize = 44)
        {
            var (peces, total) = await _pezService.GetPecesPaginadosAsync(page, pageSize);

            var respuesta = new ResultadoPaginadoDto<Pez>
            {
                Items = peces,
                TotalCount = total
            };

            return Ok(respuesta);
        }

        [HttpPost("filtrar")]
        public async Task<IActionResult> GetPecesFiltrados([FromBody] FiltroPezDto filtro, [FromQuery] int page = 1, [FromQuery] int pageSize = 44)
        {
            try
            {
                var (peces, totalCount) = await _pezService.GetPecesFiltradosAsync(filtro, page, pageSize);

                var respuesta = new ResultadoPaginadoDto<Pez>
                {
                    Items = peces,
                    TotalCount = totalCount
                };

                return Ok(respuesta);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno al filtrar los peces.");
            }
        }

    }
}
