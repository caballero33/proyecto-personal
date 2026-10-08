using TriageDoc.Domain.Entities;

namespace TriageDoc.Application.Features.Protocolos.DTOs;

public class CreateProtocoloRequestDto
{
    public NivelPrioridad NivelPrioridad { get; set; }
    public string CodigoColor { get; set; } = string.Empty;
    public string NombreNivel { get; set; } = string.Empty;
    public int TiempoMaximoEsperaMinutos { get; set; }
    public string AreaDerivacion { get; set; } = string.Empty;
    public string AccionInmediata { get; set; } = string.Empty;
}

public class UpdateProtocoloRequestDto
{
    public NivelPrioridad NivelPrioridad { get; set; }
    public string CodigoColor { get; set; } = string.Empty;
    public string NombreNivel { get; set; } = string.Empty;
    public int TiempoMaximoEsperaMinutos { get; set; }
    public string AreaDerivacion { get; set; } = string.Empty;
    public string AccionInmediata { get; set; } = string.Empty;
}

public class ProtocoloResponseDto
{
    public int Id { get; set; }
    public NivelPrioridad NivelPrioridad { get; set; }
    public string CodigoColor { get; set; } = string.Empty;
    public string NombreNivel { get; set; } = string.Empty;
    public int TiempoMaximoEsperaMinutos { get; set; }
    public string AreaDerivacion { get; set; } = string.Empty;
    public string AccionInmediata { get; set; } = string.Empty;
}
