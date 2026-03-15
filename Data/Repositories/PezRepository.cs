using Data.Context;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories
{
    public class PezRepository : IPezRepository
    {
        private readonly AquaVivariumContext _context;

        public PezRepository(AquaVivariumContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Pez>> GetAllAsync()
        {
            return await _context.Peces.Include(p => p.Especie).ToListAsync();
        }

        public async Task AddAsync(Pez pez)
        {
            _context.Peces.Add(pez);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
