namespace TriageDoc.Application.Features.Pacientes.DTOs;

public class CreatePacienteRequestDto
{
    public string Dni { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public int Edad { get; set; }
    public string Genero { get; set; } = "No Especificado";
    public string? Telefono { get; set; }
}

public class UpdatePacienteRequestDto
{
    public string Dni { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public int Edad { get; set; }
    public string Genero { get; set; } = "No Especificado";
    public string? Telefono { get; set; }
}

public class PacienteResponseDto
{
    public int Id { get; set; }
    public string Dni { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public int Edad { get; set; }
    public string Genero { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public DateTime FechaRegistro { get; set; }
    public int TotalEvaluaciones { get; set; }
}
