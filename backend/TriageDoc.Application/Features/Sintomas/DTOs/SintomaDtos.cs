namespace TriageDoc.Application.Features.Sintomas.DTOs;

public class CreateSintomaRequestDto
{
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public bool EsSignoAlarma { get; set; }
    public string Categoria { get; set; } = "General";
    public int NivelGravedadBase { get; set; } = 3;
}

public class UpdateSintomaRequestDto
{
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public bool EsSignoAlarma { get; set; }
    public string Categoria { get; set; } = "General";
    public int NivelGravedadBase { get; set; } = 3;
}

public class SintomaResponseDto
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public bool EsSignoAlarma { get; set; }
    public string Categoria { get; set; } = string.Empty;
    public int NivelGravedadBase { get; set; }
}
