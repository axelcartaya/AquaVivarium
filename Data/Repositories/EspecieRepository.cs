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

        public async Task AddImagenAsync(EspecieImagen imagen)
        {
            await _context.EspecieImagenes.AddAsync(imagen);
            await _context.SaveChangesAsync();
        }
        public async Task<List<EspecieImagen>> GetImagenesByEspecieIdAsync(int especieId)
        {
            return await _context.EspecieImagenes
                .Where(x => x.EspecieId == especieId)
                .OrderBy(x => x.Url)
                .ToListAsync();
        }
        public async Task AddConsultaAsync(EspecieConsulta consulta)
        {
            _context.EspecieConsultas.Add(consulta);
            await _context.SaveChangesAsync();
        }

        public async Task AddRespuestaAsync(EspecieRespuesta respuesta)
        {
            _context.EspecieRespuestas.Add(respuesta);
            await _context.SaveChangesAsync();
        }

        public async Task<List<EspecieConsulta>> GetConsultasByEspecieIdAsync(int especieId)
        {
            return await _context.EspecieConsultas
                .Include(c => c.EspecieRespuesta)
                .Where(c => c.EspecieId == especieId)
                .OrderByDescending(c => c.FechaPublicacion)
                .ToListAsync();
        }
    }
}
