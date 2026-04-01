using Domain.Interfaces.Repositories;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces.Services
{
  
    public interface IEspecieService
    {
        Task AddConsultaAsync(EspecieConsulta consulta);
        Task AddRespuestaAsync(EspecieRespuesta respuesta);
    }
}
