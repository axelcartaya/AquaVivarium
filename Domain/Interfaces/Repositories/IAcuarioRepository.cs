using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces.Repositories
{
    public interface IAcuarioRepository
    {
        Task<Acuario?> GetByIdAsync(int id);
        Task<IEnumerable<Acuario>> GetByUsuarioIdAsync(string usuarioId);
        Task<Acuario> AddAsync(Acuario acuario);
        Task UpdateAsync(Acuario acuario);
        Task DeleteAsync(int id);
        Task<bool> SaveChangesAsync();
    }
}
