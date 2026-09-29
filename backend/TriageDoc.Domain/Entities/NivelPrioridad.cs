namespace TriageDoc.Domain.Entities;

/// <summary>
/// Niveles de prioridad estandarizados de Triage (Sistema Manchester).
/// </summary>
public enum NivelPrioridad
{
    Resucitacion = 1, // Rojo: Atención inmediata
    Emergencia = 2,   // Naranja: Muy urgente (< 10-15 min)
    Urgencia = 3,     // Amarillo: Urgente (< 60 min)
    Prioritario = 4,  // Verde: Estándar / Poco urgente (< 120 min)
    NoUrgente = 5     // Azul: No urgente (< 240 min)
}
