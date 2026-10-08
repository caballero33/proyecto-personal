using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TriageDoc.Application.Features.Protocolos.DTOs;
using TriageDoc.Application.Interfaces;
using TriageDoc.Domain.Entities;

namespace TriageDoc.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProtocolosController : ControllerBase
{
    private readonly IProtocoloAtencionRepository _protocoloRepository;
    private readonly IValidator<CreateProtocoloRequestDto> _createValidator;
    private readonly IValidator<UpdateProtocoloRequestDto> _updateValidator;

    public ProtocolosController(
        IProtocoloAtencionRepository protocoloRepository,
        IValidator<CreateProtocoloRequestDto> createValidator,
        IValidator<UpdateProtocoloRequestDto> updateValidator)
    {
        _protocoloRepository = protocoloRepository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    /// <summary>
    /// Consulta todos los protocolos de atención médica clasificados por nivel Manchester.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProtocoloResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var protocolos = await _protocoloRepository.GetAllAsync();
        var dtos = protocolos.Select(p => new ProtocoloResponseDto
        {
            Id = p.Id,
            NivelPrioridad = p.NivelPrioridad,
            CodigoColor = p.CodigoColor,
            NombreNivel = p.NombreNivel,
            TiempoMaximoEsperaMinutos = p.TiempoMaximoEsperaMinutos,
            AreaDerivacion = p.AreaDerivacion,
            AccionInmediata = p.AccionInmediata
        });

        return Ok(dtos);
    }

    /// <summary>
    /// Consulta un protocolo específico por su ID.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProtocoloResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var p = await _protocoloRepository.GetByIdAsync(id);
        if (p is null)
        {
            return NotFound(new { mensaje = $"No se encontró el protocolo de atención con ID {id}." });
        }

        var dto = new ProtocoloResponseDto
        {
            Id = p.Id,
            NivelPrioridad = p.NivelPrioridad,
            CodigoColor = p.CodigoColor,
            NombreNivel = p.NombreNivel,
            TiempoMaximoEsperaMinutos = p.TiempoMaximoEsperaMinutos,
            AreaDerivacion = p.AreaDerivacion,
            AccionInmediata = p.AccionInmediata
        };

        return Ok(dto);
    }

    /// <summary>
    /// Crea un nuevo protocolo de atención clínica.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ProtocoloResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateProtocoloRequestDto dto)
    {
        var validationResult = await _createValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            return BadRequest(new { errores = validationResult.Errors.Select(e => e.ErrorMessage) });
        }

        var protocolo = new ProtocoloAtencion
        {
            NivelPrioridad = dto.NivelPrioridad,
            CodigoColor = dto.CodigoColor.Trim(),
            NombreNivel = dto.NombreNivel.Trim(),
            TiempoMaximoEsperaMinutos = dto.TiempoMaximoEsperaMinutos,
            AreaDerivacion = dto.AreaDerivacion.Trim(),
            AccionInmediata = dto.AccionInmediata.Trim()
        };

        var creado = await _protocoloRepository.AddAsync(protocolo);

        var responseDto = new ProtocoloResponseDto
        {
            Id = creado.Id,
            NivelPrioridad = creado.NivelPrioridad,
            CodigoColor = creado.CodigoColor,
            NombreNivel = creado.NombreNivel,
            TiempoMaximoEsperaMinutos = creado.TiempoMaximoEsperaMinutos,
            AreaDerivacion = creado.AreaDerivacion,
            AccionInmediata = creado.AccionInmediata
        };

        return CreatedAtAction(nameof(GetById), new { id = creado.Id }, responseDto);
    }

    /// <summary>
    /// Modifica las reglas de un protocolo existente.
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProtocoloRequestDto dto)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            return BadRequest(new { errores = validationResult.Errors.Select(e => e.ErrorMessage) });
        }

        var p = await _protocoloRepository.GetByIdAsync(id);
        if (p is null)
        {
            return NotFound(new { mensaje = $"No se encontró el protocolo con ID {id}." });
        }

        p.NivelPrioridad = dto.NivelPrioridad;
        p.CodigoColor = dto.CodigoColor.Trim();
        p.NombreNivel = dto.NombreNivel.Trim();
        p.TiempoMaximoEsperaMinutos = dto.TiempoMaximoEsperaMinutos;
        p.AreaDerivacion = dto.AreaDerivacion.Trim();
        p.AccionInmediata = dto.AccionInmediata.Trim();

        await _protocoloRepository.UpdateAsync(p);

        return NoContent();
    }

    /// <summary>
    /// Elimina un protocolo de atención clínica.
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var eliminado = await _protocoloRepository.DeleteAsync(id);
        if (!eliminado)
        {
            return NotFound(new { mensaje = $"No se encontró el protocolo con ID {id}." });
        }

        return NoContent();
    }
}
