using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces.Services
{
    public interface IPezService
    {
        Task<IEnumerable<Pez>> GetAllPecesAsync();
        Task<Pez?> GetPezByIdAsync(int id);
        Task<(IEnumerable<Pez> Peces, int Total)> GetPecesPaginadosAsync(int page, int pageSize);
    }
}
