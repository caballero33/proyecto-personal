using Microsoft.AspNetCore.Mvc;
using TriageDoc.API.DTOs;
using TriageDoc.Application.Interfaces;

namespace TriageDoc.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TriageController : ControllerBase
{
    private readonly ITriageService _triageService;

    // Inyección de dependencias a través del constructor (DIP)
    public TriageController(ITriageService triageService)
    {
        _triageService = triageService;
    }

    /// <summary>
    /// Verifica el estado operativo del motor experto de triage.
    /// </summary>
    [HttpGet("estado")]
    public IActionResult ObtenerEstado()
    {
        var estado = _triageService.ObtenerEstadoMotor();
        return Ok(new { estado, fecha = DateTime.UtcNow });
    }

    /// <summary>
    /// Procesa una evaluación de síntomas aplicando las reglas clínicas del sistema experto.
    /// </summary>
    [HttpPost("evaluar")]
    public IActionResult EvaluarCaso([FromBody] ConsultaTriageDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.NombrePaciente))
        {
            return BadRequest(new { mensaje = "El nombre del paciente es requerido." });
        }

        var resultado = _triageService.EvaluarCasoPreliminar(
            dto.NombrePaciente,
            dto.Edad,
            dto.DolorPecho,
            dto.DificultadRespiratoria,
            dto.Temperatura
        );

        return Ok(resultado);
    }
}
