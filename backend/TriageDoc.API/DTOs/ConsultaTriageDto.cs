namespace TriageDoc.API.DTOs;

public class ConsultaTriageDto
{
    public string NombrePaciente { get; set; } = string.Empty;
    public int Edad { get; set; }
    public bool DolorPecho { get; set; }
    public bool DificultadRespiratoria { get; set; }
    public double Temperatura { get; set; } = 36.5;
}
