using Data.Context;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories
{
    public class EspecieRepository : IEspecieRepository
    {
        private readonly AquaVivariumContext _context;

        public EspecieRepository(AquaVivariumContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Especie>> GetAllAsync()
        {
            return await _context.Especies.ToListAsync();
        }

        public async Task AddAsync(Especie especie)
        {
            _context.Especies.Add(especie);
            await _context.SaveChangesAsync();
        }
    }
}
