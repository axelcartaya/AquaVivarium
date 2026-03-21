using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;

namespace AquaVivarium.Services
{
    public class PezService : IPezService
    {
        private readonly IPezRepository _repository;

        public PezService(IPezRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Pez>> GetAllPecesAsync()
            => await _repository.GetAllAsync();

        public async Task<Pez?> GetPezByIdAsync(int id)
            => await _repository.GetPezByIdAsync(id);
    }
}
