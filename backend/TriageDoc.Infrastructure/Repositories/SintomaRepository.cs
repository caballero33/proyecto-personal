using Microsoft.EntityFrameworkCore;
using TriageDoc.Application.Interfaces;
using TriageDoc.Domain.Entities;
using TriageDoc.Infrastructure.Data;

namespace TriageDoc.Infrastructure.Repositories;

/// <summary>
/// Implementación con Entity Framework Core del Repositorio de Síntomas.
/// </summary>
public class SintomaRepository : ISintomaRepository
{
    private readonly TriageDocDbContext _context;

    public SintomaRepository(TriageDocDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Sintoma>> GetAllAsync()
    {
        return await _context.Sintomas
            .AsNoTracking()
            .OrderBy(s => s.Nombre)
            .ToListAsync();
    }

    public async Task<Sintoma?> GetByIdAsync(int id)
    {
        return await _context.Sintomas.FindAsync(id);
    }

    public async Task<Sintoma?> GetByCodigoAsync(string codigo)
    {
        return await _context.Sintomas
            .FirstOrDefaultAsync(s => s.Codigo == codigo);
    }

    public async Task<IEnumerable<Sintoma>> GetSignosAlarmaAsync()
    {
        return await _context.Sintomas
            .AsNoTracking()
            .Where(s => s.EsSignoAlarma)
            .ToListAsync();
    }

    public async Task<Sintoma> AddAsync(Sintoma sintoma)
    {
        await _context.Sintomas.AddAsync(sintoma);
        await _context.SaveChangesAsync();
        return sintoma;
    }

    public async Task<bool> UpdateAsync(Sintoma sintoma)
    {
        var existing = await _context.Sintomas.FindAsync(sintoma.Id);
        if (existing is null) return false;

        existing.Codigo = sintoma.Codigo;
        existing.Nombre = sintoma.Nombre;
        existing.Descripcion = sintoma.Descripcion;
        existing.EsSignoAlarma = sintoma.EsSignoAlarma;
        existing.Categoria = sintoma.Categoria;
        existing.NivelGravedadBase = sintoma.NivelGravedadBase;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var sintoma = await _context.Sintomas.FindAsync(id);
        if (sintoma is null) return false;

        _context.Sintomas.Remove(sintoma);
        await _context.SaveChangesAsync();
        return true;
    }
}
