using Domain.Models;
using Domain.Models.DTOs;

namespace Domain.Interfaces.Services
{
  
    public interface IEspecieService
    {
        Task AddConsultaAsync(EspecieConsulta consulta);
        Task AddRespuestaAsync(EspecieRespuesta respuesta);
        Task<IEnumerable<EspecieBusquedaDto>> BuscarEspeciesAsync(string nombreEspecie);
    }
}
