namespace TriageDoc.Domain.Entities;

/// <summary>
/// Entidad de Dominio que representa el registro de una consulta o evaluación realizada por el motor de inferencia.
/// Vincula al paciente con sus signos vitales medidos y el veredicto del sistema experto.
/// </summary>
public class EvaluacionTriage
{
    public int Id { get; set; }

    // Llave foránea hacia Paciente (3FN)
    public int PacienteId { get; set; }
    public Paciente? Paciente { get; set; }

    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    // Signos Vitales y Triaje Clínico
    public double Temperatura { get; set; }
    public int FrecuenciaCardiaca { get; set; }
    public string PresionArterial { get; set; } = string.Empty;
    public int SaturacionOxigeno { get; set; }
    public bool DolorToracico { get; set; }
    public bool DificultadRespiratoria { get; set; }
    public string SintomasPrincipales { get; set; } = string.Empty;

    // Veredicto del Motor Experto
    public NivelPrioridad PrioridadAsignada { get; set; }
    public int? ProtocoloAtencionId { get; set; }
    public ProtocoloAtencion? Protocolo { get; set; }
    public string JustificacionRegla { get; set; } = string.Empty;
    public bool RequiereAtencionInmediata { get; set; }
    public string ObservacionesClinicas { get; set; } = string.Empty;
}
