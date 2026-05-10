using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;

namespace AquaVivarium.Services
{
    public class GuiaService : IGuiaService
    {
        private readonly IGuiaRepository _repository;
        private readonly IWebHostEnvironment _env;

        public GuiaService(IGuiaRepository repository, IWebHostEnvironment env)
        {
            _repository = repository;
            _env = env;
        }

        public async Task<IEnumerable<Guia>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Guia?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Guia> AddAsync(Guia estilo)
        {
            return await _repository.AddAsync(estilo);
        }

        public async Task UpdateAsync(Guia estilo)
        {
            await _repository.UpdateAsync(estilo);
        }

        public async Task DeleteAsync(int id)
        {
            var guia = await _repository.GetByIdAsync(id);

            if (guia == null) return;

            var urlsABorrar = new List<string?>();
            if (!string.IsNullOrEmpty(guia.ImagenPortadaUrl))
            {
                urlsABorrar.Add(guia.ImagenPortadaUrl);
            }
            if (guia.Imagenes != null)
            {
                urlsABorrar.AddRange(guia.Imagenes.Select(img => img.Url));
            }

            await _repository.DeleteAsync(id);

            // borra la imagenes del servidor
            foreach (var url in urlsABorrar)
            {
                if (!string.IsNullOrEmpty(url))
                {
                    var rutaFisicaCompleta = Path.Combine(_env.WebRootPath, url.TrimStart('/'));
                    if (File.Exists(rutaFisicaCompleta))
                    {
                        File.Delete(rutaFisicaCompleta);
                    }
                }
            }
        }
        public async Task<IEnumerable<Guia>> GetByCategoriaIdAsync(int categoriaId)
        {
            return await _repository.GetByCategoriaIdAsync(categoriaId);
        }
    }
}
