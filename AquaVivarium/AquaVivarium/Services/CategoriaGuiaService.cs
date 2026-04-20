using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;

namespace AquaVivarium.Services
{
    public class CategoriaGuiaService : ICategoriaGuiaService
    {
        private readonly ICategoriaGuiaRepository _repository;

        public CategoriaGuiaService(ICategoriaGuiaRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CategoriaGuia>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<CategoriaGuia?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<CategoriaGuia> AddAsync(CategoriaGuia estilo)
        {
            return await _repository.AddAsync(estilo);
        }

        public async Task UpdateAsync(CategoriaGuia estilo)
        {
            await _repository.UpdateAsync(estilo);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
