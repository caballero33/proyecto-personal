using TriageDoc.Domain.Entities;

namespace TriageDoc.Application.Interfaces;

/// <summary>
/// Contrato del Repositorio de Protocolos de Atención Manchester (Principio DIP).
/// </summary>
public interface IProtocoloAtencionRepository
{
    Task<IEnumerable<ProtocoloAtencion>> GetAllAsync();
    Task<ProtocoloAtencion?> GetByIdAsync(int id);
    Task<ProtocoloAtencion?> GetByNivelPrioridadAsync(NivelPrioridad prioridad);
    Task<ProtocoloAtencion> AddAsync(ProtocoloAtencion protocolo);
    Task<bool> UpdateAsync(ProtocoloAtencion protocolo);
    Task<bool> DeleteAsync(int id);
}
