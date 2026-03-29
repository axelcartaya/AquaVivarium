using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces.Repositories
{
    public interface IEspecieRepository
    {
        Task<IEnumerable<Especie>> GetAllAsync();
        Task AddAsync(Especie especie);
        Task AddImagenAsync(EspecieImagen imagen);
        Task<List<EspecieImagen>> GetImagenesByEspecieIdAsync(int especieId);
    }
}
