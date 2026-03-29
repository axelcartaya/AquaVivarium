using Domain.Interfaces.Services;
using Domain.Models;

namespace AquaVivarium.Services
{
    public class EspecieService : IEspecieService
    {
        public Task<List<EspecieImagen>> GetImagenesAsync(int especieId)
        {
            throw new NotImplementedException();
        }
    }
}
