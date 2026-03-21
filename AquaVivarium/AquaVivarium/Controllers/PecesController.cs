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

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var peces = await _pezService.GetAllPecesAsync();
            return Ok(peces);
        }
    }
}
