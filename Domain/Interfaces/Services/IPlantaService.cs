using Domain.Models;
using Domain.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces.Services
{
    public interface IPlantaService
    {
        Task<IEnumerable<Planta>> GetAllPlantasAsync();
        Task<Planta?> GetPlantaByIdAsync(int id);
        Task<(IEnumerable<Planta> Plantas, int Total)> GetPlantasPaginadasAsync(int page, int pageSize);
        Task<(IEnumerable<Planta> Plantas, int TotalCount)> GetPlantasFiltradasAsync(FiltroPlantaDto filtro, int page, int pageSize);
    }
}
