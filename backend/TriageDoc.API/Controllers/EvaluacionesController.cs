using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TriageDoc.Application.Features.Evaluaciones.DTOs;
using TriageDoc.Application.Interfaces;

namespace TriageDoc.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EvaluacionesController : ControllerBase
{
    private readonly IEvaluacionTriageRepository _evaluacionRepository;
    private readonly IPacienteRepository _pacienteRepository;
    private readonly IProtocoloAtencionRepository _protocoloRepository;
    private readonly ITriageService _triageService;
    private readonly IValidator<CreateEvaluacionRequestDto> _createValidator;

    public EvaluacionesController(
        IEvaluacionTriageRepository evaluacionRepository,
        IPacienteRepository pacienteRepository,
        IProtocoloAtencionRepository protocoloRepository,
        ITriageService triageService,
        IValidator<CreateEvaluacionRequestDto> createValidator)
    {
        _evaluacionRepository = evaluacionRepository;
        _pacienteRepository = pacienteRepository;
        _protocoloRepository = protocoloRepository;
        _triageService = triageService;
        _createValidator = createValidator;
    }

    /// <summary>
    /// Consulta el historial completo de evaluaciones y triajes médicos realizados.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<EvaluacionResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var evaluaciones = await _evaluacionRepository.GetAllAsync();
        var dtos = evaluaciones.Select(e => new EvaluacionResponseDto
        {
            Id = e.Id,
            PacienteId = e.PacienteId,
            NombrePaciente = e.Paciente?.NombreCompleto ?? "No Registrado",
            EdadPaciente = e.Paciente?.Edad ?? 0,
            FechaRegistro = e.FechaRegistro,
            Temperatura = e.Temperatura,
            FrecuenciaCardiaca = e.FrecuenciaCardiaca,
            PresionArterial = e.PresionArterial,
            SaturacionOxigeno = e.SaturacionOxigeno,
            DolorToracico = e.DolorToracico,
            DificultadRespiratoria = e.DificultadRespiratoria,
            SintomasPrincipales = e.SintomasPrincipales,
            PrioridadAsignada = e.PrioridadAsignada,
            CodigoColor = e.Protocolo?.CodigoColor ?? "N/A",
            NombrePrioridad = e.Protocolo?.NombreNivel ?? e.PrioridadAsignada.ToString(),
            TiempoMaximoEsperaMinutos = e.Protocolo?.TiempoMaximoEsperaMinutos ?? 0,
            AreaDerivacion = e.Protocolo?.AreaDerivacion ?? "Área General",
            AccionInmediata = e.Protocolo?.AccionInmediata ?? string.Empty,
            JustificacionRegla = e.JustificacionRegla,
            RequiereAtencionInmediata = e.RequiereAtencionInmediata,
            ObservacionesClinicas = e.ObservacionesClinicas
        });

        return Ok(dtos);
    }

    /// <summary>
    /// Consulta una evaluación específica de triage por su ID.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(EvaluacionResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var e = await _evaluacionRepository.GetByIdAsync(id);
        if (e is null)
        {
            return NotFound(new { mensaje = $"No se encontró la evaluación con ID {id}." });
        }

        var dto = new EvaluacionResponseDto
        {
            Id = e.Id,
            PacienteId = e.PacienteId,
            NombrePaciente = e.Paciente?.NombreCompleto ?? "No Registrado",
            EdadPaciente = e.Paciente?.Edad ?? 0,
            FechaRegistro = e.FechaRegistro,
            Temperatura = e.Temperatura,
            FrecuenciaCardiaca = e.FrecuenciaCardiaca,
            PresionArterial = e.PresionArterial,
            SaturacionOxigeno = e.SaturacionOxigeno,
            DolorToracico = e.DolorToracico,
            DificultadRespiratoria = e.DificultadRespiratoria,
            SintomasPrincipales = e.SintomasPrincipales,
            PrioridadAsignada = e.PrioridadAsignada,
            CodigoColor = e.Protocolo?.CodigoColor ?? "N/A",
            NombrePrioridad = e.Protocolo?.NombreNivel ?? e.PrioridadAsignada.ToString(),
            TiempoMaximoEsperaMinutos = e.Protocolo?.TiempoMaximoEsperaMinutos ?? 0,
            AreaDerivacion = e.Protocolo?.AreaDerivacion ?? "Área General",
            AccionInmediata = e.Protocolo?.AccionInmediata ?? string.Empty,
            JustificacionRegla = e.JustificacionRegla,
            RequiereAtencionInmediata = e.RequiereAtencionInmediata,
            ObservacionesClinicas = e.ObservacionesClinicas
        };

        return Ok(dto);
    }

    /// <summary>
    /// Consulta el historial de triajes de un paciente determinado.
    /// </summary>
    [HttpGet("paciente/{pacienteId:int}")]
    [ProducesResponseType(typeof(IEnumerable<EvaluacionResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByPaciente(int pacienteId)
    {
        var evaluaciones = await _evaluacionRepository.GetByPacienteIdAsync(pacienteId);
        var paciente = await _pacienteRepository.GetByIdAsync(pacienteId);

        var dtos = evaluaciones.Select(e => new EvaluacionResponseDto
        {
            Id = e.Id,
            PacienteId = e.PacienteId,
            NombrePaciente = paciente?.NombreCompleto ?? "Desconocido",
            EdadPaciente = paciente?.Edad ?? 0,
            FechaRegistro = e.FechaRegistro,
            Temperatura = e.Temperatura,
            FrecuenciaCardiaca = e.FrecuenciaCardiaca,
            PresionArterial = e.PresionArterial,
            SaturacionOxigeno = e.SaturacionOxigeno,
            DolorToracico = e.DolorToracico,
            DificultadRespiratoria = e.DificultadRespiratoria,
            SintomasPrincipales = e.SintomasPrincipales,
            PrioridadAsignada = e.PrioridadAsignada,
            CodigoColor = e.Protocolo?.CodigoColor ?? "N/A",
            NombrePrioridad = e.Protocolo?.NombreNivel ?? e.PrioridadAsignada.ToString(),
            TiempoMaximoEsperaMinutos = e.Protocolo?.TiempoMaximoEsperaMinutos ?? 0,
            AreaDerivacion = e.Protocolo?.AreaDerivacion ?? "Área General",
            AccionInmediata = e.Protocolo?.AccionInmediata ?? string.Empty,
            JustificacionRegla = e.JustificacionRegla,
            RequiereAtencionInmediata = e.RequiereAtencionInmediata,
            ObservacionesClinicas = e.ObservacionesClinicas
        });

        return Ok(dtos);
    }

    /// <summary>
    /// Procesa la evaluación clínica mediante el Motor Experto y persiste el diagnóstico en PostgreSQL.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(EvaluacionResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EvaluarPaciente([FromBody] CreateEvaluacionRequestDto dto)
    {
        var validationResult = await _createValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            return BadRequest(new { errores = validationResult.Errors.Select(e => e.ErrorMessage) });
        }

        // 1. Verificar existencia del paciente
        var paciente = await _pacienteRepository.GetByIdAsync(dto.PacienteId);
        if (paciente is null)
        {
            return NotFound(new { mensaje = $"No existe el paciente con ID {dto.PacienteId}." });
        }

        // 2. Disparar el Motor de Inferencia Experto (Clean Architecture & SOLID)
        var evaluacion = _triageService.ProcesarInferencia(paciente.Id, paciente.Edad, dto);

        // 3. Obtener el Protocolo Manchester correspondiente
        var protocolo = await _protocoloRepository.GetByNivelPrioridadAsync(evaluacion.PrioridadAsignada);
        if (protocolo is not null)
        {
            evaluacion.ProtocoloAtencionId = protocolo.Id;
        }

        // 4. Persistir en la base de datos
        var guardada = await _evaluacionRepository.AddAsync(evaluacion);

        // 5. Mapear DTO de Salida
        var responseDto = new EvaluacionResponseDto
        {
            Id = guardada.Id,
            PacienteId = paciente.Id,
            NombrePaciente = paciente.NombreCompleto,
            EdadPaciente = paciente.Edad,
            FechaRegistro = guardada.FechaRegistro,
            Temperatura = guardada.Temperatura,
            FrecuenciaCardiaca = guardada.FrecuenciaCardiaca,
            PresionArterial = guardada.PresionArterial,
            SaturacionOxigeno = guardada.SaturacionOxigeno,
            DolorToracico = guardada.DolorToracico,
            DificultadRespiratoria = guardada.DificultadRespiratoria,
            SintomasPrincipales = guardada.SintomasPrincipales,
            PrioridadAsignada = guardada.PrioridadAsignada,
            CodigoColor = protocolo?.CodigoColor ?? "N/A",
            NombrePrioridad = protocolo?.NombreNivel ?? guardada.PrioridadAsignada.ToString(),
            TiempoMaximoEsperaMinutos = protocolo?.TiempoMaximoEsperaMinutos ?? 0,
            AreaDerivacion = protocolo?.AreaDerivacion ?? "Área General",
            AccionInmediata = protocolo?.AccionInmediata ?? string.Empty,
            JustificacionRegla = guardada.JustificacionRegla,
            RequiereAtencionInmediata = guardada.RequiereAtencionInmediata,
            ObservacionesClinicas = guardada.ObservacionesClinicas
        };

        return CreatedAtAction(nameof(GetById), new { id = guardada.Id }, responseDto);
    }

    /// <summary>
    /// Elimina una evaluación de triage registrada.
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var eliminado = await _evaluacionRepository.DeleteAsync(id);
        if (!eliminado)
        {
            return NotFound(new { mensaje = $"No se encontró la evaluación con ID {id} para eliminar." });
        }

        return NoContent();
    }
}
