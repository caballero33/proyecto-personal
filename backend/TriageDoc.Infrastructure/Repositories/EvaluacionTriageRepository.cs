using Microsoft.EntityFrameworkCore;
using TriageDoc.Application.Interfaces;
using TriageDoc.Domain.Entities;
using TriageDoc.Infrastructure.Data;

namespace TriageDoc.Infrastructure.Repositories;

/// <summary>
/// Implementación con Entity Framework Core del Repositorio de Evaluaciones de Triage.
/// Incluye las relaciones con Paciente y ProtocoloAtencion.
/// </summary>
public class EvaluacionTriageRepository : IEvaluacionTriageRepository
{
    private readonly TriageDocDbContext _context;

    public EvaluacionTriageRepository(TriageDocDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<EvaluacionTriage>> GetAllAsync()
    {
        return await _context.EvaluacionesTriage
            .AsNoTracking()
            .Include(e => e.Paciente)
            .Include(e => e.Protocolo)
            .OrderByDescending(e => e.FechaRegistro)
            .ToListAsync();
    }

    public async Task<EvaluacionTriage?> GetByIdAsync(int id)
    {
        return await _context.EvaluacionesTriage
            .Include(e => e.Paciente)
            .Include(e => e.Protocolo)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<EvaluacionTriage>> GetByPacienteIdAsync(int pacienteId)
    {
        return await _context.EvaluacionesTriage
            .AsNoTracking()
            .Where(e => e.PacienteId == pacienteId)
            .Include(e => e.Protocolo)
            .OrderByDescending(e => e.FechaRegistro)
            .ToListAsync();
    }

    public async Task<EvaluacionTriage> AddAsync(EvaluacionTriage evaluacion)
    {
        await _context.EvaluacionesTriage.AddAsync(evaluacion);
        await _context.SaveChangesAsync();
        return evaluacion;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var evaluacion = await _context.EvaluacionesTriage.FindAsync(id);
        if (evaluacion is null) return false;

        _context.EvaluacionesTriage.Remove(evaluacion);
        await _context.SaveChangesAsync();
        return true;
    }
}
