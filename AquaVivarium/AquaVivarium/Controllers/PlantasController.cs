using Domain.Interfaces.Services;
using Domain.Models;
using Domain.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AquaVivarium.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlantasController : ControllerBase
    {
        private readonly IPlantaService _plantaService;

        public PlantasController(IPlantaService plantaService)
        {
            _plantaService = plantaService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var planta = await _plantaService.GetPlantaByIdAsync(id);
            if (planta == null) return NotFound();
            return Ok(planta);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var plantas = await _plantaService.GetAllPlantasAsync();
            return Ok(plantas);
        }

        [HttpGet("paginadas")]
        public async Task<IActionResult> GetPlantasPaginadas([FromQuery] int page = 1, [FromQuery] int pageSize = 44)
        {
            var (plantas, total) = await _plantaService.GetPlantasPaginadasAsync(page, pageSize);

            var respuesta = new ResultadoPaginadoDto<Planta>
            {
                Items = plantas,
                TotalCount = total
            };

            return Ok(respuesta);
        }

        [HttpPost("filtrar")]
        public async Task<IActionResult> GetPlantasFiltradas([FromBody] FiltroPlantaDto filtro, [FromQuery] int page = 1, [FromQuery] int pageSize = 44)
        {
            try
            {
                var (plantas, totalCount) = await _plantaService.GetPlantasFiltradasAsync(filtro, page, pageSize);

                var respuesta = new ResultadoPaginadoDto<Planta>
                {
                    Items = plantas,
                    TotalCount = totalCount
                };

                return Ok(respuesta);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno al filtrar las plantas.");
            }
        }
    }
}
