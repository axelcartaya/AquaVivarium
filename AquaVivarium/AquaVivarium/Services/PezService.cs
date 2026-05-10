using Data.Repositories;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;
using Domain.Models.DTOs;

namespace AquaVivarium.Services
{
    public class PezService : IPezService
    {
        private readonly IPezRepository _repository;

        private const int DefaultPageSize = 44;
        private const int MaxPageSize = 100;

        public PezService(IPezRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Pez>> GetAllPecesAsync()
            => await _repository.GetAllAsync();

        public async Task<Pez?> GetPezByIdAsync(int id)
            => await _repository.GetPezByIdAsync(id);

        public async Task<(IEnumerable<Pez> Peces, int Total)> GetPecesPaginadosAsync(int page, int pageSize)
        {
            (page, pageSize) = ValidarPaginacion(page, pageSize);
            return await _repository.GetPecesPaginadosAsync(page, pageSize);
        }

        public async Task<(IEnumerable<Pez> Peces, int TotalCount)> GetPecesFiltradosAsync(FiltroPezDto filtro, int page, int pageSize)
        {
           (page, pageSize) = ValidarPaginacion(page, pageSize);
            return await _repository.GetPecesFiltradosAsync(filtro, page, pageSize);
        }


        //método para asegurar que no se pueden acceder a páginas negativas ni mostrar números gigantezcos de peces por página para evitar ataques DDOS
       private (int page, int pageSize) ValidarPaginacion(int page, int pageSize)
        {
            if (page < 1) page = 1;

            if (pageSize < 1) pageSize = DefaultPageSize;
            else if (pageSize > MaxPageSize) pageSize = MaxPageSize;

            return (page, pageSize);
        }
    }
}
   
