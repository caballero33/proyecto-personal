namespace TriageDoc.Domain.Entities;

/// <summary>
/// Representa el registro de una consulta o evaluación realizada por el motor de inferencia.
/// </summary>
public class EvaluacionTriage
{
    public int Id { get; set; }
    public string NombrePaciente { get; set; } = string.Empty;
    public int Edad { get; set; }
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
    public NivelPrioridad PrioridadAsignada { get; set; }
    public string JustificacionRegla { get; set; } = string.Empty;
    public bool RequiereAtencionInmediata { get; set; }
}
