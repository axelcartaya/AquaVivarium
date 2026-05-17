using Data.Context;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories
{
    public class GuiaRepository : IGuiaRepository
    {
        private readonly AquaVivariumContext _context;

        public GuiaRepository(AquaVivariumContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Guia>> GetAllAsync()
        {
            return await _context.Guias
                .Include(i => i.Imagenes)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Guia?> GetByIdAsync(int id)
        {
            return await _context.Guias
                .Include(i => i.Imagenes)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Guia> AddAsync(Guia guia)
        {
            _context.Guias.Add(guia);
            await _context.SaveChangesAsync();
            return guia;
        }

        public async Task UpdateAsync(Guia guia)
        {
            _context.Guias.Update(guia);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var guia = await _context.Guias.FindAsync(id);
            if (guia != null)
            {
                _context.Guias.Remove(guia);
                await _context.SaveChangesAsync();
            }
        }
      
        public async Task<IEnumerable<Guia>> GetByCategoriaIdAsync(int categoriaId)
        {
            return await _context.Guias
                .Include(g => g.Imagenes) 
                .Where(g => g.CategoriaGuiaId == categoriaId)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
