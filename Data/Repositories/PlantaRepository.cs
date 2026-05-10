using Data.Context;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Domain.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories
{
    public class PlantaRepository : IPlantaRepository
    {
        private readonly AquaVivariumContext _context;

        public PlantaRepository(AquaVivariumContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Planta>> GetAllAsync()
        {
            return await _context.Plantas
                .Include(p => p.Especie)
                    .ThenInclude(e => e.EspecieImagenes)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(Planta planta)
        {
            _context.Plantas.Add(planta);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Planta?> GetPlantaByIdAsync(int id)
        {
            var planta = await _context.Plantas
                .Include(p => p.Especie)
                    .ThenInclude(e => e.EspecieImagenes)
                .Include(p => p.Especie)
                    .ThenInclude(e => e.EspecieConsulta)
                        .ThenInclude(c => c.EspecieRespuesta)
                .FirstOrDefaultAsync(p => p.EspecieId == id); 

            if (planta?.Especie?.EspecieConsulta == null || !planta.Especie.EspecieConsulta.Any())
                return planta;

            // Traducción de nombre de usuario
            var userIds = planta.Especie.EspecieConsulta.Select(c => c.UsuarioId)
                .Union(planta.Especie.EspecieConsulta.SelectMany(c => c.EspecieRespuesta).Select(r => r.UsuarioId))
                .Distinct()
                .ToList();

            var diccionarioUsuarios = await _context.Users
                .Where(u => userIds.Contains(u.Id))
                .ToDictionaryAsync(
                    u => u.Id,
                    u => !string.IsNullOrEmpty(u.Alias) ? u.Alias : u.UserName.Split('@')[0]
                );

            foreach (var consulta in planta.Especie.EspecieConsulta)
            {
                consulta.NombreUsuario = diccionarioUsuarios.GetValueOrDefault(consulta.UsuarioId);

                foreach (var respuesta in consulta.EspecieRespuesta)
                {
                    respuesta.NombreUsuario = diccionarioUsuarios.GetValueOrDefault(respuesta.UsuarioId);
                }
            }

            return planta;
        }

        public async Task<(IEnumerable<Planta> Plantas, int Total)> GetPlantasPaginadasAsync(int page, int pageSize)
        {
            var query = _context.Plantas
                .Include(p => p.Especie)
                    .ThenInclude(e => e.EspecieImagenes)
                .AsNoTracking();

            int total = await query.CountAsync();

            var plantas = await query
                .OrderBy(p => p.EspecieId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (plantas, total);
        }

        public async Task<(IEnumerable<Planta> Plantas, int TotalCount)> GetPlantasFiltradasAsync(FiltroPlantaDto filtro, int page, int pageSize)
        {
            var query = _context.Plantas
                .Include(p => p.Especie)
                    .ThenInclude(e => e.EspecieImagenes)
                .AsQueryable();

            // Filtros base de Especie
            if (filtro.PhDesde.HasValue) query = query.Where(p => p.Especie.PhMax >= filtro.PhDesde.Value);
            if (filtro.PhHasta.HasValue) query = query.Where(p => p.Especie.PhMin <= filtro.PhHasta.Value);
            if (filtro.TempDesde.HasValue) query = query.Where(p => p.Especie.TempMax >= filtro.TempDesde.Value);
            if (filtro.TempHasta.HasValue) query = query.Where(p => p.Especie.TempMin <= filtro.TempHasta.Value);
            if (filtro.GhDesde.HasValue) query = query.Where(p => p.Especie.GhMax >= filtro.GhDesde.Value);
            if (filtro.GhHasta.HasValue) query = query.Where(p => p.Especie.GhMin <= filtro.GhHasta.Value);

            if (!string.IsNullOrWhiteSpace(filtro.Dificultad)) query = query.Where(p => p.Especie.Dificultad == filtro.Dificultad);
            if (!string.IsNullOrWhiteSpace(filtro.Nombre)) query = query.Where(p => p.Especie.Nombre.Contains(filtro.Nombre));
            if (!string.IsNullOrWhiteSpace(filtro.Familia)) query = query.Where(p => p.Especie.Familia.Contains(filtro.Familia));
            if (!string.IsNullOrWhiteSpace(filtro.Origen)) query = query.Where(p => p.Especie.Origen.Contains(filtro.Origen));

            // Filtros específicos de Planta
            if (!string.IsNullOrWhiteSpace(filtro.Iluminacion)) query = query.Where(p => p.Iluminacion == filtro.Iluminacion);
            if (!string.IsNullOrWhiteSpace(filtro.Crecimiento)) query = query.Where(p => p.Crecimiento == filtro.Crecimiento);
            if (filtro.NecesitaCo2.HasValue) query = query.Where(p => p.NecesitaCo2 == filtro.NecesitaCo2.Value);

            var totalCount = await query.CountAsync();

            var plantas = await query
                .OrderBy(p => p.Especie.Nombre)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (plantas, totalCount);
        }
    }
}

