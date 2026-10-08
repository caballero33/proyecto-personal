using FluentValidation;
using TriageDoc.Application.Features.Evaluaciones.DTOs;

namespace TriageDoc.Application.Features.Evaluaciones.Validators;

public class CreateEvaluacionValidator : AbstractValidator<CreateEvaluacionRequestDto>
{
    public CreateEvaluacionValidator()
    {
        RuleFor(x => x.PacienteId)
            .GreaterThan(0).WithMessage("Debe seleccionar un ID de paciente válido.");

        RuleFor(x => x.Temperatura)
            .InclusiveBetween(30.0, 45.0).WithMessage("La temperatura corporal debe oscilar entre 30.0 °C y 45.0 °C.");

        RuleFor(x => x.FrecuenciaCardiaca)
            .InclusiveBetween(30, 250).WithMessage("La frecuencia cardíaca debe oscilar entre 30 y 250 lpm.");

        RuleFor(x => x.SaturacionOxigeno)
            .InclusiveBetween(50, 100).WithMessage("La saturación de oxígeno (SpO2) debe oscilar entre 50% y 100%. ");
    }
}
