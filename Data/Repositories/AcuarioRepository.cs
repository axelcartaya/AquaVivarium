using Data.Context;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories
{
    public class AcuarioRepository : IAcuarioRepository
    {
        private readonly AquaVivariumContext _context;

        public AcuarioRepository(AquaVivariumContext context)
        {
            _context = context;
        }

        public async Task<Acuario?> GetByIdAsync(int id)
        {
            return await _context.Acuarios
                .Include(a => a.AcuarioEspecies)
                    .ThenInclude(ae => ae.Especie)
                        .ThenInclude(e => e.Pez)
                .Include(a => a.AcuarioEspecies)
                    .ThenInclude(ae => ae.Especie)
                        .ThenInclude(e => e.Planta)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Acuario>> GetByUsuarioIdAsync(string usuarioId)
        {
            return await _context.Acuarios
                .Include(a => a.AcuarioEspecies)
                    .ThenInclude(ae => ae.Especie)
                        .ThenInclude(e => e.Pez)
                .Include(a => a.AcuarioEspecies)
                    .ThenInclude(ae => ae.Especie)
                        .ThenInclude(e => e.Planta)
                .Where(a => a.UsuarioId == usuarioId)
                .ToListAsync();
        }

        public async Task<Acuario> AddAsync(Acuario acuario)
        {
            _context.Acuarios.Add(acuario);
            await _context.SaveChangesAsync();
            return acuario;
        }

        public async Task UpdateAsync(Acuario acuario)
        {
            _context.Acuarios.Update(acuario);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var acuario = await _context.Acuarios.FindAsync(id);
            if (acuario != null)
            {
                _context.Acuarios.Remove(acuario);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
