using Data.Context;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

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
            return await _context.Peces
                .Include(p => p.Especie)
                    .ThenInclude(e => e.EspecieImagenes) 
                .AsNoTracking()
                .ToListAsync();
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

        public async Task<Pez> GetPezByIdAsync(int id)
        {
            return await _context.Peces.Include(p => p.Especie).FirstOrDefaultAsync(p => p.EspecieId == id);
        }

        public async Task<(IEnumerable<Pez> Peces, int Total)> GetPecesPaginadosAsync(int page, int pageSize)
        {
            var query = _context.Peces
                .Include(p => p.Especie)
                .ThenInclude(e => e.EspecieImagenes)
                .AsNoTracking();

            int total = await query.CountAsync();

            var peces = await query
                .OrderBy(p => p.EspecieId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (peces, total);
        }
    }
}
