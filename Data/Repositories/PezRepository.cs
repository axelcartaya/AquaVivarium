using Data.Context;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Domain.Models.DTOs;
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
            var pez = await _context.Peces
            .Include(p => p.Especie)
                .ThenInclude(e => e.EspecieImagenes)
            .Include(p => p.Especie)
                .ThenInclude(e => e.EspecieConsulta)
                    .ThenInclude(c => c.EspecieRespuesta)
            .FirstOrDefaultAsync(p => p.Especie.Id == id);

            if (pez?.Especie?.EspecieConsulta == null) return pez;

            //Traducción de nombre de usuario para obtener su alias y no su GUID en el apartado Comunidad
            var userIds = pez.Especie.EspecieConsulta.Select(c => c.UsuarioId)
                .Union(pez.Especie.EspecieConsulta.SelectMany(c => c.EspecieRespuesta).Select(r => r.UsuarioId))
                .Distinct()
                .ToList();

            var diccionarioUsuarios = await _context.Users
                .Where(u => userIds.Contains(u.Id))
                .ToDictionaryAsync(
                    u => u.Id,
                    u => !string.IsNullOrEmpty(u.Alias) ? u.Alias : u.UserName.Split('@')[0] //Primero se mira si el usuario tiene asignado un alias, si no se coge la parte del nombre del correo
                );

            foreach (var consulta in pez.Especie.EspecieConsulta)
            {
                consulta.NombreUsuario = diccionarioUsuarios.GetValueOrDefault(consulta.UsuarioId);

                foreach (var respuesta in consulta.EspecieRespuesta)
                {
                    respuesta.NombreUsuario = diccionarioUsuarios.GetValueOrDefault(respuesta.UsuarioId);
                }
            }

            return pez;

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

        public async Task<(IEnumerable<Pez> Peces, int TotalCount)> GetPecesFiltradosAsync(FiltroPezDto filtro, int page, int pageSize)
        {
            var query = _context.Peces
                .Include(p => p.Especie)
                    .ThenInclude(e => e.EspecieImagenes)
                .AsQueryable();

            //filtro de especies
            if (!string.IsNullOrWhiteSpace(filtro.Nombre))query = query.Where(p => p.Especie.Nombre.Contains(filtro.Nombre));
            if (!string.IsNullOrWhiteSpace(filtro.Familia))query = query.Where(p => p.Especie.Familia.Contains(filtro.Familia));
            if (!string.IsNullOrWhiteSpace(filtro.Genero))query = query.Where(p => p.Especie.Genero.Contains(filtro.Genero));
            if (filtro.PhDesde.HasValue)query = query.Where(p => p.Especie.PhMin >= filtro.PhDesde.Value);
            if (filtro.PhHasta.HasValue)query = query.Where(p => p.Especie.PhMax <= filtro.PhHasta.Value);
            if (filtro.TempDesde.HasValue)query = query.Where(p => p.Especie.TempMin >= filtro.TempDesde.Value);
            if (filtro.TempHasta.HasValue)query = query.Where(p => p.Especie.TempMax <= filtro.TempHasta.Value);
            if (filtro.GhDesde.HasValue)query = query.Where(p => p.Especie.GhMin >= filtro.GhDesde.Value);
            if (filtro.GhHasta.HasValue)query = query.Where(p => p.Especie.GhMax <= filtro.GhHasta.Value);

            // filtros de peces
            if (filtro.TamanoDesde.HasValue)query = query.Where(p => p.TamanoMaxCm >= filtro.TamanoDesde.Value);
            if (filtro.TamanoHasta.HasValue)query = query.Where(p => p.TamanoMaxCm <= filtro.TamanoHasta.Value);
            if (!string.IsNullOrWhiteSpace(filtro.Alimentacion))query = query.Where(p => p.Alimentacion == filtro.Alimentacion);

            // paginación
            var totalCount = await query.CountAsync();
 
            var peces = await query
                .OrderBy(p => p.Especie.Nombre) 
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (peces, totalCount);
        }

    }
}
