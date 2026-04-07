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

        public async Task<IEnumerable<EspecieBusquedaDto>> BuscarEspeciesAsync(string nombreEspecie)
        {
            if (string.IsNullOrWhiteSpace(nombreEspecie))
                return new List<EspecieBusquedaDto>();

            nombreEspecie = nombreEspecie.ToLower().Trim();

            return await _context.Especies
                .Where(e => e.Nombre.ToLower().Contains(nombreEspecie) ||
                            (e.NombreCientifico != null && e.NombreCientifico.ToLower().Contains(nombreEspecie)))
                .Select(e => new EspecieBusquedaDto
                {
                    Id = e.Id,
                    Nombre = e.Nombre,
                    NombreCientifico = e.NombreCientifico,
                    TipoEspecie = e.TipoEspecie,
                    ImagenUrl = e.EspecieImagenes
                                 .OrderBy(i => i.Id)
                                 .Select(i => i.Url)
                                 .FirstOrDefault()
                })
                .Take(10) 
                .ToListAsync();
        }
    }
}
