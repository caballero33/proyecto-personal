using Microsoft.AspNetCore.Mvc;
using TriageDoc.Application.Interfaces;

namespace TriageDoc.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TriageController : ControllerBase
{
    private readonly ITriageService _triageService;

    public TriageController(ITriageService triageService)
    {
        _triageService = triageService;
    }

    /// <summary>
    /// Verifica la disponibilidad y estado de calibración del motor experto de triage.
    /// </summary>
    [HttpGet("estado")]
    public IActionResult ObtenerEstado()
    {
        var estado = _triageService.ObtenerEstadoMotor();
        return Ok(new
        {
            estado,
            motor = "InferenceEngine-Manchester-v2",
            timestamp = DateTime.UtcNow,
            tablasCore = new[] { "Pacientes", "Sintomas", "ProtocolosAtencion", "EvaluacionesTriage" }
        });
    }
}
