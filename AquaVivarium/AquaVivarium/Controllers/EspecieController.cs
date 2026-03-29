using Domain.Interfaces.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AquaVivarium.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EspecieController : ControllerBase
    {
        private readonly IEspecieRepository _especieRepo;

        public EspecieController(IEspecieRepository especieRepo)
        {
            _especieRepo = especieRepo;
        }

        [HttpGet("{especieId}/imagenes")]
        public async Task<ActionResult<List<EspecieImagen>>> GetImagenes(int especieId)
        {
            var imagenes = await _especieRepo.GetImagenesByEspecieIdAsync(especieId);

            return Ok(imagenes ?? new List<EspecieImagen>());
        }
    }
}
