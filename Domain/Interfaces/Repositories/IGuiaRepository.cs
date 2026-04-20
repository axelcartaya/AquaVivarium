using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces.Repositories
{
    public interface IGuiaRepository
    {
        Task<IEnumerable<Guia>> GetAllAsync();
        Task<Guia?> GetByIdAsync(int id);
        Task<Guia> AddAsync(Guia estilo);
        Task UpdateAsync(Guia estilo);
        Task DeleteAsync(int id);
    }
}
