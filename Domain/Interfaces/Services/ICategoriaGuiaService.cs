
using Domain.Models;

namespace Domain.Interfaces.Services
{
    public interface ICategoriaGuiaService
    {
        Task<IEnumerable<CategoriaGuia>> GetAllAsync();
        Task<CategoriaGuia?> GetByIdAsync(int id);
        Task<CategoriaGuia> AddAsync(CategoriaGuia estilo);
        Task UpdateAsync(CategoriaGuia estilo);
        Task DeleteAsync(int id);
    }
}
