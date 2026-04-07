using Domain.Models;
using Domain.Models.DTOs;
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
        Task AddConsultaAsync(EspecieConsulta consulta);
        Task AddRespuestaAsync(EspecieRespuesta respuesta);
        Task<List<EspecieConsulta>> GetConsultasByEspecieIdAsync(int especieId);
        Task<IEnumerable<EspecieBusquedaDto>> BuscarEspeciesAsync(string nombreEspecie);
    }
}
