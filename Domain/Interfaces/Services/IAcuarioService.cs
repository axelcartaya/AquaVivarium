using Domain.Models;
using Domain.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces.Services
{
    public interface IAcuarioService
    {
        Task<Acuario?> GetByIdAsync(int id);
        Task<IEnumerable<Acuario>> GetByUsuarioIdAsync(string usuarioId);
        Task<Acuario> AddAsync(Acuario acuario);
        Task<Acuario?> AddDesdeDtoAsync(AcuarioTransferDto dto);
        Task UpdateAsync(Acuario acuario);
        Task DeleteAsync(int id);
        Task<List<Acuario>> GetMisAcuariosAsync();
        Task<bool> UpdateDesdeDtoAsync(int id, AcuarioTransferDto dto);
    }
}
