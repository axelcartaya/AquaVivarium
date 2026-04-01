using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;

namespace AquaVivarium.Services
{
    public class EspecieService : IEspecieService
    {
        private readonly IEspecieRepository _repository;
        public EspecieService(IEspecieRepository repository) {
            _repository = repository;
        }
        public async Task AddConsultaAsync(EspecieConsulta consulta)
        {
            await _repository.AddConsultaAsync(consulta);
        }

        public async Task AddRespuestaAsync(EspecieRespuesta respuesta)
        {
            await _repository.AddRespuestaAsync(respuesta);
        }
    }
}
