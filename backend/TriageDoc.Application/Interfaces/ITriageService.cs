using TriageDoc.Domain.Entities;

namespace TriageDoc.Application.Interfaces;

/// <summary>
/// Contrato de Abstracción (Principio DIP - La 'D' de SOLID).
/// Define las operaciones que expone el motor experto de triage a las capas externas.
/// </summary>
public interface ITriageService
{
    string ObtenerEstadoMotor();
    EvaluacionTriage EvaluarCasoPreliminar(string nombrePaciente, int edad, bool dolorPecho, bool dificultadRespiratoria, double temperatura);
}
