using Domain.Models;

namespace Domain.Interfaces.Repositories
{
    public interface ICategoriaGuiaRepository
    {
        Task<IEnumerable<CategoriaGuia>> GetAllAsync();
        Task<CategoriaGuia?> GetByIdAsync(int id);
        Task<CategoriaGuia> AddAsync(CategoriaGuia estilo);
        Task UpdateAsync(CategoriaGuia estilo);
        Task DeleteAsync(int id);
    }
}
