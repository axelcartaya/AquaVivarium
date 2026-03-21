using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces.Repositories
{
    public interface IPezRepository
    {
        Task<IEnumerable<Pez>> GetAllAsync();
        Task<Pez> GetPezByIdAsync(int id);
        Task AddAsync(Pez pez);
        Task SaveChangesAsync(); //Para guardar el JSon
    }
}
