using Domain.Interfaces.Services;
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
        public async Task<ActionResult> GetPaginados(int page = 1, int pageSize = 44)
        {
            var (peces, total) = await _pezService.GetPecesPaginadosAsync(page, pageSize);

            return Ok(new { Peces = peces, Total = total });
        }


    }
}
