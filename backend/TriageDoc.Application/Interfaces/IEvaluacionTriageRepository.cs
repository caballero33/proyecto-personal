using TriageDoc.Domain.Entities;

namespace TriageDoc.Application.Interfaces;

/// <summary>
/// Contrato del Repositorio de Evaluaciones de Triage (Principio DIP).
/// </summary>
public interface IEvaluacionTriageRepository
{
    Task<IEnumerable<EvaluacionTriage>> GetAllAsync();
    Task<EvaluacionTriage?> GetByIdAsync(int id);
    Task<IEnumerable<EvaluacionTriage>> GetByPacienteIdAsync(int pacienteId);
    Task<EvaluacionTriage> AddAsync(EvaluacionTriage evaluacion);
    Task<bool> DeleteAsync(int id);
}
