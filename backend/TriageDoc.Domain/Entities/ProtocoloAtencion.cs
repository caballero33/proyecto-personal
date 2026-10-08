namespace TriageDoc.Domain.Entities;

/// <summary>
/// Entidad de Dominio que define los protocolos de atención estandarizados (Manchester Triage System).
/// Especifica el tiempo objetivo de atención, el área física de derivación y las acciones inmediatas.
/// </summary>
public class ProtocoloAtencion
{
    public int Id { get; set; }
    public NivelPrioridad NivelPrioridad { get; set; }
    public string CodigoColor { get; set; } = string.Empty; // Rojo, Naranja, Amarillo, Verde, Azul
    public string NombreNivel { get; set; } = string.Empty; // Resucitación, Emergencia, etc.
    public int TiempoMaximoEsperaMinutos { get; set; }
    public string AreaDerivacion { get; set; } = string.Empty; // Sala de Reanimación / Shock Trauma, etc.
    public string AccionInmediata { get; set; } = string.Empty;
}
