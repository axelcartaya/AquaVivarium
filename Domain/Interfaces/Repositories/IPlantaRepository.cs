using Domain.Models;
using Domain.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces.Repositories
{
    public interface IPlantaRepository
    {
        Task<IEnumerable<Planta>> GetAllAsync();
        Task<Planta?> GetPlantaByIdAsync(int id);
        Task AddAsync(Planta planta);
        Task SaveChangesAsync();
        Task<(IEnumerable<Planta> Plantas, int Total)> GetPlantasPaginadasAsync(int pagina, int tamañoPagina);
        Task<(IEnumerable<Planta> Plantas, int TotalCount)> GetPlantasFiltradasAsync(FiltroPlantaDto filtro, int page, int pageSize);
    }
}

