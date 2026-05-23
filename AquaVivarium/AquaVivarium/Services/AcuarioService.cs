using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;
using Domain.Models.DTOs;

namespace AquaVivarium.Services
{
    public class AcuarioService : IAcuarioService
    {
        private readonly IAcuarioRepository _acuarioRepository;

        public AcuarioService(IAcuarioRepository acuarioRepository)
        {
            _acuarioRepository = acuarioRepository;
        }

        public async Task<Acuario?> GetByIdAsync(int id)
        {
            return await _acuarioRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Acuario>> GetByUsuarioIdAsync(string usuarioId)
        {
            return await _acuarioRepository.GetByUsuarioIdAsync(usuarioId);
        }

        public async Task<Acuario> AddAsync(Acuario acuario)
        {
            return await _acuarioRepository.AddAsync(acuario);
        }

        public async Task UpdateAsync(Acuario acuario)
        {
            await _acuarioRepository.UpdateAsync(acuario);
        }

        public async Task DeleteAsync(int id)
        {
            await _acuarioRepository.DeleteAsync(id);
        }
        public Task<Acuario?> AddDesdeDtoAsync(AcuarioTransferDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<List<Acuario>> GetMisAcuariosAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateDesdeDtoAsync(int id, AcuarioTransferDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
