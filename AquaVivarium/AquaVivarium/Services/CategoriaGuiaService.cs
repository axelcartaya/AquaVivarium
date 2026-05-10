using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;

namespace AquaVivarium.Services
{
    public class CategoriaGuiaService : ICategoriaGuiaService
    {
        private readonly ICategoriaGuiaRepository _repository;
        private readonly IWebHostEnvironment _env;

        public CategoriaGuiaService(ICategoriaGuiaRepository repository, IWebHostEnvironment env)
        {
            _repository = repository;
            _env = env;
        }

        public async Task<IEnumerable<CategoriaGuia>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<CategoriaGuia?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<CategoriaGuia> AddAsync(CategoriaGuia estilo)
        {
            return await _repository.AddAsync(estilo);
        }

        public async Task UpdateAsync(CategoriaGuia estilo)
        {
            await _repository.UpdateAsync(estilo);
        }

        public async Task DeleteAsync(int id)
        {
            var categoria = await _repository.GetByIdAsync(id);

            if (categoria == null) return;

            if (categoria.Guias != null && categoria.Guias.Any())          
                throw new InvalidOperationException("No se puede eliminar una categoría que contiene guías. Por favor, reasigna o elimina las guías primero.");
           
            await _repository.DeleteAsync(id);

            if (!string.IsNullOrEmpty(categoria.ImagenPortadaUrl))
            {
                var rutaFisica = Path.Combine(_env.WebRootPath, categoria.ImagenPortadaUrl.TrimStart('/'));
                if (File.Exists(rutaFisica))
                {
                    File.Delete(rutaFisica);
                }
            }
        }
    }
}
