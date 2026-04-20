using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;

namespace AquaVivarium.Services
{
    public class GuiaService : IGuiaService
    {
        private readonly IGuiaRepository _repository;

        public GuiaService(IGuiaRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Guia>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Guia?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Guia> AddAsync(Guia estilo)
        {
            return await _repository.AddAsync(estilo);
        }

        public async Task UpdateAsync(Guia estilo)
        {
            await _repository.UpdateAsync(estilo);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
