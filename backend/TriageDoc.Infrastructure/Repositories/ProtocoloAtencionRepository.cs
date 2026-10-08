using Microsoft.EntityFrameworkCore;
using TriageDoc.Application.Interfaces;
using TriageDoc.Domain.Entities;
using TriageDoc.Infrastructure.Data;

namespace TriageDoc.Infrastructure.Repositories;

/// <summary>
/// Implementación con Entity Framework Core del Repositorio de Protocolos de Atención.
/// </summary>
public class ProtocoloAtencionRepository : IProtocoloAtencionRepository
{
    private readonly TriageDocDbContext _context;

    public ProtocoloAtencionRepository(TriageDocDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProtocoloAtencion>> GetAllAsync()
    {
        return await _context.ProtocolosAtencion
            .AsNoTracking()
            .OrderBy(p => p.NivelPrioridad)
            .ToListAsync();
    }

    public async Task<ProtocoloAtencion?> GetByIdAsync(int id)
    {
        return await _context.ProtocolosAtencion.FindAsync(id);
    }

    public async Task<ProtocoloAtencion?> GetByNivelPrioridadAsync(NivelPrioridad prioridad)
    {
        return await _context.ProtocolosAtencion
            .FirstOrDefaultAsync(p => p.NivelPrioridad == prioridad);
    }

    public async Task<ProtocoloAtencion> AddAsync(ProtocoloAtencion protocolo)
    {
        await _context.ProtocolosAtencion.AddAsync(protocolo);
        await _context.SaveChangesAsync();
        return protocolo;
    }

    public async Task<bool> UpdateAsync(ProtocoloAtencion protocolo)
    {
        var existing = await _context.ProtocolosAtencion.FindAsync(protocolo.Id);
        if (existing is null) return false;

        existing.NivelPrioridad = protocolo.NivelPrioridad;
        existing.CodigoColor = protocolo.CodigoColor;
        existing.NombreNivel = protocolo.NombreNivel;
        existing.TiempoMaximoEsperaMinutos = protocolo.TiempoMaximoEsperaMinutos;
        existing.AreaDerivacion = protocolo.AreaDerivacion;
        existing.AccionInmediata = protocolo.AccionInmediata;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var protocolo = await _context.ProtocolosAtencion.FindAsync(id);
        if (protocolo is null) return false;

        _context.ProtocolosAtencion.Remove(protocolo);
        await _context.SaveChangesAsync();
        return true;
    }
}
