using TriageDoc.Domain.Entities;

namespace TriageDoc.Application.Features.Evaluaciones.DTOs;

public class CreateEvaluacionRequestDto
{
    public int PacienteId { get; set; }
    public double Temperatura { get; set; }
    public int FrecuenciaCardiaca { get; set; }
    public string PresionArterial { get; set; } = "120/80";
    public int SaturacionOxigeno { get; set; } = 98;
    public bool DolorToracico { get; set; }
    public bool DificultadRespiratoria { get; set; }
    public string SintomasPrincipales { get; set; } = string.Empty;
    public string ObservacionesClinicas { get; set; } = string.Empty;
}

public class EvaluacionResponseDto
{
    public int Id { get; set; }
    public int PacienteId { get; set; }
    public string NombrePaciente { get; set; } = string.Empty;
    public int EdadPaciente { get; set; }
    public DateTime FechaRegistro { get; set; }

    // Signos Vitales
    public double Temperatura { get; set; }
    public int FrecuenciaCardiaca { get; set; }
    public string PresionArterial { get; set; } = string.Empty;
    public int SaturacionOxigeno { get; set; }
    public bool DolorToracico { get; set; }
    public bool DificultadRespiratoria { get; set; }
    public string SintomasPrincipales { get; set; } = string.Empty;

    // Clasificación Experta y Protocolo
    public NivelPrioridad PrioridadAsignada { get; set; }
    public string CodigoColor { get; set; } = string.Empty;
    public string NombrePrioridad { get; set; } = string.Empty;
    public int TiempoMaximoEsperaMinutos { get; set; }
    public string AreaDerivacion { get; set; } = string.Empty;
    public string AccionInmediata { get; set; } = string.Empty;
    public string JustificacionRegla { get; set; } = string.Empty;
    public bool RequiereAtencionInmediata { get; set; }
    public string ObservacionesClinicas { get; set; } = string.Empty;
}
