namespace TriageDoc.Domain.Entities;

/// <summary>
/// Entidad de Dominio que representa un síntoma evaluable por el motor experto de triage.
/// </summary>
public class Sintoma
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public bool EsSignoAlarma { get; set; }
    public string Categoria { get; set; } = "General";
    public int NivelGravedadBase { get; set; } = 3;
}
