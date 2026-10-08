using TriageDoc.Domain.Entities;

namespace TriageDoc.Application.Interfaces;

/// <summary>
/// Contrato del Repositorio de Síntomas (Principio DIP).
/// </summary>
public interface ISintomaRepository
{
    Task<IEnumerable<Sintoma>> GetAllAsync();
    Task<Sintoma?> GetByIdAsync(int id);
    Task<Sintoma?> GetByCodigoAsync(string codigo);
    Task<IEnumerable<Sintoma>> GetSignosAlarmaAsync();
    Task<Sintoma> AddAsync(Sintoma sintoma);
    Task<bool> UpdateAsync(Sintoma sintoma);
    Task<bool> DeleteAsync(int id);
}
