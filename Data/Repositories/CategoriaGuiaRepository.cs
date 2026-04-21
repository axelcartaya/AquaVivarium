using Data.Context;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class CategoriaGuiaRepository : ICategoriaGuiaRepository
    {
        private readonly AquaVivariumContext _context;

        public CategoriaGuiaRepository(AquaVivariumContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoriaGuia>> GetAllAsync()
        {
            return await _context.CategoriasGuia
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<CategoriaGuia?> GetByIdAsync(int id)
        {
            return await _context.CategoriasGuia
                .Include(c => c.Guias) 
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<CategoriaGuia> AddAsync(CategoriaGuia categoriaGuia)
        {
            _context.CategoriasGuia.Add(categoriaGuia);
            await _context.SaveChangesAsync();
            return categoriaGuia;
        }

        public async Task UpdateAsync(CategoriaGuia categoriaGuia)
        {
            _context.CategoriasGuia.Update(categoriaGuia);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var categoriaGuia = await _context.CategoriasGuia.FindAsync(id);
            if (categoriaGuia != null)
            {
                _context.CategoriasGuia.Remove(categoriaGuia);
                await _context.SaveChangesAsync();
            }
        }
    
    }
}
