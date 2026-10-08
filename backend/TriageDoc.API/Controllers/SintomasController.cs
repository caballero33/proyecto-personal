using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TriageDoc.Application.Features.Sintomas.DTOs;
using TriageDoc.Application.Interfaces;
using TriageDoc.Domain.Entities;

namespace TriageDoc.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SintomasController : ControllerBase
{
    private readonly ISintomaRepository _sintomaRepository;
    private readonly IValidator<CreateSintomaRequestDto> _createValidator;
    private readonly IValidator<UpdateSintomaRequestDto> _updateValidator;

    public SintomasController(
        ISintomaRepository sintomaRepository,
        IValidator<CreateSintomaRequestDto> createValidator,
        IValidator<UpdateSintomaRequestDto> updateValidator)
    {
        _sintomaRepository = sintomaRepository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    /// <summary>
    /// Consulta el catálogo completo de síntomas clínicos.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SintomaResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var sintomas = await _sintomaRepository.GetAllAsync();
        var dtos = sintomas.Select(s => new SintomaResponseDto
        {
            Id = s.Id,
            Codigo = s.Codigo,
            Nombre = s.Nombre,
            Descripcion = s.Descripcion,
            EsSignoAlarma = s.EsSignoAlarma,
            Categoria = s.Categoria,
            NivelGravedadBase = s.NivelGravedadBase
        });

        return Ok(dtos);
    }

    /// <summary>
    /// Filtra exclusivamente los síntomas clasificados como signos de alarma (Banderas Rojas).
    /// </summary>
    [HttpGet("alarmas")]
    [ProducesResponseType(typeof(IEnumerable<SintomaResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSignosAlarma()
    {
        var alarmas = await _sintomaRepository.GetSignosAlarmaAsync();
        var dtos = alarmas.Select(s => new SintomaResponseDto
        {
            Id = s.Id,
            Codigo = s.Codigo,
            Nombre = s.Nombre,
            Descripcion = s.Descripcion,
            EsSignoAlarma = s.EsSignoAlarma,
            Categoria = s.Categoria,
            NivelGravedadBase = s.NivelGravedadBase
        });

        return Ok(dtos);
    }

    /// <summary>
    /// Consulta un síntoma específico por su ID.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(SintomaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var s = await _sintomaRepository.GetByIdAsync(id);
        if (s is null)
        {
            return NotFound(new { mensaje = $"No se encontró ningún síntoma con el ID {id}." });
        }

        var dto = new SintomaResponseDto
        {
            Id = s.Id,
            Codigo = s.Codigo,
            Nombre = s.Nombre,
            Descripcion = s.Descripcion,
            EsSignoAlarma = s.EsSignoAlarma,
            Categoria = s.Categoria,
            NivelGravedadBase = s.NivelGravedadBase
        };

        return Ok(dto);
    }

    /// <summary>
    /// Agrega un nuevo síntoma al catálogo clínico.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(SintomaResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateSintomaRequestDto dto)
    {
        var validationResult = await _createValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            return BadRequest(new { errores = validationResult.Errors.Select(e => e.ErrorMessage) });
        }

        var existe = await _sintomaRepository.GetByCodigoAsync(dto.Codigo);
        if (existe is not null)
        {
            return BadRequest(new { mensaje = $"Ya existe un síntoma con el código {dto.Codigo}." });
        }

        var sintoma = new Sintoma
        {
            Codigo = dto.Codigo.Trim(),
            Nombre = dto.Nombre.Trim(),
            Descripcion = dto.Descripcion.Trim(),
            EsSignoAlarma = dto.EsSignoAlarma,
            Categoria = dto.Categoria.Trim(),
            NivelGravedadBase = dto.NivelGravedadBase
        };

        var creado = await _sintomaRepository.AddAsync(sintoma);

        var responseDto = new SintomaResponseDto
        {
            Id = creado.Id,
            Codigo = creado.Codigo,
            Nombre = creado.Nombre,
            Descripcion = creado.Descripcion,
            EsSignoAlarma = creado.EsSignoAlarma,
            Categoria = creado.Categoria,
            NivelGravedadBase = creado.NivelGravedadBase
        };

        return CreatedAtAction(nameof(GetById), new { id = creado.Id }, responseDto);
    }

    /// <summary>
    /// Modifica los datos de un síntoma existente.
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSintomaRequestDto dto)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            return BadRequest(new { errores = validationResult.Errors.Select(e => e.ErrorMessage) });
        }

        var s = await _sintomaRepository.GetByIdAsync(id);
        if (s is null)
        {
            return NotFound(new { mensaje = $"No se encontró el síntoma con ID {id}." });
        }

        s.Codigo = dto.Codigo.Trim();
        s.Nombre = dto.Nombre.Trim();
        s.Descripcion = dto.Descripcion.Trim();
        s.EsSignoAlarma = dto.EsSignoAlarma;
        s.Categoria = dto.Categoria.Trim();
        s.NivelGravedadBase = dto.NivelGravedadBase;

        await _sintomaRepository.UpdateAsync(s);

        return NoContent();
    }

    /// <summary>
    /// Elimina un síntoma del catálogo.
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var eliminado = await _sintomaRepository.DeleteAsync(id);
        if (!eliminado)
        {
            return NotFound(new { mensaje = $"No se encontró el síntoma con ID {id}." });
        }

        return NoContent();
    }
}
