namespace TriageDoc.Domain.Entities;

/// <summary>
/// Entidad de Dominio que representa a un paciente ingresado para clasificación médica.
/// Cumple con la normalización relacional (3FN) aislando los datos de filiación.
/// </summary>
public class Paciente
{
    public int Id { get; set; }
    public string Dni { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public int Edad { get; set; }
    public string Genero { get; set; } = "No Especificado";
    public string? Telefono { get; set; }
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    // Relación de navegación: Un paciente puede tener múltiples evaluaciones de triage
    public ICollection<EvaluacionTriage> Evaluaciones { get; set; } = new List<EvaluacionTriage>();
}
