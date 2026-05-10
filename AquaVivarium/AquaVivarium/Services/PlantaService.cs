using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;
using Domain.Models.DTOs;

namespace AquaVivarium.Services
{
    public class PlantaService : IPlantaService
    {
        private readonly IPlantaRepository _repository;

        private const int DefaultPageSize = 44;
        private const int MaxPageSize = 100;

        public PlantaService(IPlantaRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Planta>> GetAllPlantasAsync()
            => await _repository.GetAllAsync();

        public async Task<Planta?> GetPlantaByIdAsync(int id)
            => await _repository.GetPlantaByIdAsync(id);

        public async Task<(IEnumerable<Planta> Plantas, int Total)> GetPlantasPaginadasAsync(int page, int pageSize)
        {
            (page, pageSize) = ValidarPaginacion(page, pageSize);
            return await _repository.GetPlantasPaginadasAsync(page, pageSize);
        }

        public async Task<(IEnumerable<Planta> Plantas, int TotalCount)> GetPlantasFiltradasAsync(FiltroPlantaDto filtro, int page, int pageSize)
        {
            (page, pageSize) = ValidarPaginacion(page, pageSize);
            return await _repository.GetPlantasFiltradasAsync(filtro, page, pageSize);
        }

        private (int page, int pageSize) ValidarPaginacion(int page, int pageSize)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = DefaultPageSize;
            else if (pageSize > MaxPageSize) pageSize = MaxPageSize;
            return (page, pageSize);
        }
    }
}

