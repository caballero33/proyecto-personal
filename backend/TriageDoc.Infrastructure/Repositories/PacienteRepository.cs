using Microsoft.EntityFrameworkCore;
using TriageDoc.Application.Interfaces;
using TriageDoc.Domain.Entities;
using TriageDoc.Infrastructure.Data;

namespace TriageDoc.Infrastructure.Repositories;

/// <summary>
/// Implementación con Entity Framework Core del Repositorio de Pacientes (Clase 5).
/// </summary>
public class PacienteRepository : IPacienteRepository
{
    private readonly TriageDocDbContext _context;

    public PacienteRepository(TriageDocDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Paciente>> GetAllAsync()
    {
        return await _context.Pacientes
            .AsNoTracking()
            .OrderByDescending(p => p.FechaRegistro)
            .ToListAsync();
    }

    public async Task<Paciente?> GetByIdAsync(int id)
    {
        return await _context.Pacientes
            .Include(p => p.Evaluaciones)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Paciente?> GetByDniAsync(string dni)
    {
        return await _context.Pacientes
            .FirstOrDefaultAsync(p => p.Dni == dni);
    }

    public async Task<Paciente> AddAsync(Paciente paciente)
    {
        await _context.Pacientes.AddAsync(paciente);
        await _context.SaveChangesAsync();
        return paciente;
    }

    public async Task<bool> UpdateAsync(Paciente paciente)
    {
        var existing = await _context.Pacientes.FindAsync(paciente.Id);
        if (existing is null) return false;

        existing.Dni = paciente.Dni;
        existing.NombreCompleto = paciente.NombreCompleto;
        existing.Edad = paciente.Edad;
        existing.Genero = paciente.Genero;
        existing.Telefono = paciente.Telefono;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var paciente = await _context.Pacientes.FindAsync(id);
        if (paciente is null) return false;

        _context.Pacientes.Remove(paciente);
        await _context.SaveChangesAsync();
        return true;
    }
}
