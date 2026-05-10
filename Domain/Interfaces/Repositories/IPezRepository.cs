using Domain.Models;
using Domain.Models.DTOs;
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
        Task SaveChangesAsync(); 
        Task<(IEnumerable<Pez> Peces, int Total)> GetPecesPaginadosAsync(int pagina, int tamañoPagina);
        Task<(IEnumerable<Pez> Peces, int TotalCount)> GetPecesFiltradosAsync(FiltroPezDto filtro, int page, int pageSize);
    }
}
