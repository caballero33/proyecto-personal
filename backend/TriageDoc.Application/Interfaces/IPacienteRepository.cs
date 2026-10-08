using TriageDoc.Domain.Entities;

namespace TriageDoc.Application.Interfaces;

/// <summary>
/// Contrato del Repositorio de Pacientes (Principio DIP - La 'D' de SOLID).
/// </summary>
public interface IPacienteRepository
{
    Task<IEnumerable<Paciente>> GetAllAsync();
    Task<Paciente?> GetByIdAsync(int id);
    Task<Paciente?> GetByDniAsync(string dni);
    Task<Paciente> AddAsync(Paciente paciente);
    Task<bool> UpdateAsync(Paciente paciente);
    Task<bool> DeleteAsync(int id);
}
