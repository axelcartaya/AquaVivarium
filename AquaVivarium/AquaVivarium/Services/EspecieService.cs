using Data.Repositories;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;
using Domain.Models.DTOs;

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
        public async Task<IEnumerable<EspecieBusquedaDto>> BuscarEspeciesAsync(string nombreEspecie)
        {
            return await _repository.BuscarEspeciesAsync(nombreEspecie);
        }
        public async Task<Especie?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
