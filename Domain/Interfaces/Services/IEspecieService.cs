using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces.Services
{
    public interface IEspecieService
    {
        Task<List<EspecieImagen>> GetImagenesAsync(int especieId);
    }
}
