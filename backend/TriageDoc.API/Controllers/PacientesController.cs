using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TriageDoc.Application.Features.Pacientes.DTOs;
using TriageDoc.Application.Interfaces;
using TriageDoc.Domain.Entities;

namespace TriageDoc.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PacientesController : ControllerBase
{
    private readonly IPacienteRepository _pacienteRepository;
    private readonly IValidator<CreatePacienteRequestDto> _createValidator;
    private readonly IValidator<UpdatePacienteRequestDto> _updateValidator;

    public PacientesController(
        IPacienteRepository pacienteRepository,
        IValidator<CreatePacienteRequestDto> createValidator,
        IValidator<UpdatePacienteRequestDto> updateValidator)
    {
        _pacienteRepository = pacienteRepository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    /// <summary>

    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PacienteResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var pacientes = await _pacienteRepository.GetAllAsync();
        var dtos = pacientes.Select(p => new PacienteResponseDto
        {
            Id = p.Id,
            Dni = p.Dni,
            NombreCompleto = p.NombreCompleto,
            Edad = p.Edad,
            Genero = p.Genero,
            Telefono = p.Telefono,
            FechaRegistro = p.FechaRegistro,
            TotalEvaluaciones = p.Evaluaciones?.Count ?? 0
        });

        return Ok(dtos);
    }

    /// <summary>
    /// Consulta un paciente específico por su ID primario (Clase 9).
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PacienteResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var paciente = await _pacienteRepository.GetByIdAsync(id);
        if (paciente is null)
        {
            return NotFound(new { mensaje = $"No se encontró ningún paciente con el ID {id}." });
        }

        var dto = new PacienteResponseDto
        {
            Id = paciente.Id,
            Dni = paciente.Dni,
            NombreCompleto = paciente.NombreCompleto,
            Edad = paciente.Edad,
            Genero = paciente.Genero,
            Telefono = paciente.Telefono,
            FechaRegistro = paciente.FechaRegistro,
            TotalEvaluaciones = paciente.Evaluaciones.Count
        };

        return Ok(dto);
    }

    /// <summary>
    /// Registra un nuevo paciente con validación FluentValidation (Clase 6 y 8).
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(PacienteResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreatePacienteRequestDto dto)
    {
        var validationResult = await _createValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            return BadRequest(new { errores = validationResult.Errors.Select(e => e.ErrorMessage) });
        }

        var existeDni = await _pacienteRepository.GetByDniAsync(dto.Dni);
        if (existeDni is not null)
        {
            return BadRequest(new { mensaje = $"Ya existe un paciente registrado con el DNI {dto.Dni}." });
        }

        var paciente = new Paciente
        {
            Dni = dto.Dni.Trim(),
            NombreCompleto = dto.NombreCompleto.Trim(),
            Edad = dto.Edad,
            Genero = dto.Genero.Trim(),
            Telefono = dto.Telefono?.Trim(),
            FechaRegistro = DateTime.UtcNow
        };

        var creado = await _pacienteRepository.AddAsync(paciente);

        var responseDto = new PacienteResponseDto
        {
            Id = creado.Id,
            Dni = creado.Dni,
            NombreCompleto = creado.NombreCompleto,
            Edad = creado.Edad,
            Genero = creado.Genero,
            Telefono = creado.Telefono,
            FechaRegistro = creado.FechaRegistro,
            TotalEvaluaciones = 0
        };

        return CreatedAtAction(nameof(GetById), new { id = creado.Id }, responseDto);
    }

    /// <summary>
    /// Actualiza los datos de un paciente existente (Clase 9).
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePacienteRequestDto dto)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            return BadRequest(new { errores = validationResult.Errors.Select(e => e.ErrorMessage) });
        }

        var paciente = await _pacienteRepository.GetByIdAsync(id);
        if (paciente is null)
        {
            return NotFound(new { mensaje = $"No se encontró el paciente con ID {id} para actualizar." });
        }

        paciente.Dni = dto.Dni.Trim();
        paciente.NombreCompleto = dto.NombreCompleto.Trim();
        paciente.Edad = dto.Edad;
        paciente.Genero = dto.Genero.Trim();
        paciente.Telefono = dto.Telefono?.Trim();

        await _pacienteRepository.UpdateAsync(paciente);

        return NoContent();
    }

    /// <summary>
    /// Elimina un paciente del sistema (Clase 9).
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var eliminado = await _pacienteRepository.DeleteAsync(id);
        if (!eliminado)
        {
            return NotFound(new { mensaje = $"No se encontró el paciente con ID {id} para eliminar." });
        }

        return NoContent();
    }
}
