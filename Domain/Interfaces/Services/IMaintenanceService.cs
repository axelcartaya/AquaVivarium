using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces.Services
{
    public interface IMaintenanceService
    {
        Task<(int Creados, string Mensaje)> SincronizarImagenesEspeciesAsync();
    }
}
