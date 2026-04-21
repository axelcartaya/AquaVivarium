using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces.Services
{
    public interface IGuiaService
    {
        Task<IEnumerable<Guia>> GetAllAsync();
        Task<Guia?> GetByIdAsync(int id);
        Task<Guia> AddAsync(Guia estilo);
        Task UpdateAsync(Guia estilo);
        Task DeleteAsync(int id);
        Task<IEnumerable<Guia>> GetByCategoriaIdAsync(int categoriaId);
    }
}
