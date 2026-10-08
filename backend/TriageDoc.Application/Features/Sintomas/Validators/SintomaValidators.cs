using FluentValidation;
using TriageDoc.Application.Features.Sintomas.DTOs;

namespace TriageDoc.Application.Features.Sintomas.Validators;

public class CreateSintomaValidator : AbstractValidator<CreateSintomaRequestDto>
{
    public CreateSintomaValidator()
    {
        RuleFor(x => x.Codigo)
            .NotEmpty().WithMessage("El código del síntoma es requerido.")
            .MaximumLength(20).WithMessage("El código no debe exceder 20 caracteres.");

        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre del síntoma es requerido.")
            .MaximumLength(120).WithMessage("El nombre no debe exceder 120 caracteres.");

        RuleFor(x => x.NivelGravedadBase)
            .InclusiveBetween(1, 5).WithMessage("El nivel de gravedad debe estar entre 1 (mínimo) y 5 (máximo).");
    }
}

public class UpdateSintomaValidator : AbstractValidator<UpdateSintomaRequestDto>
{
    public UpdateSintomaValidator()
    {
        RuleFor(x => x.Codigo)
            .NotEmpty().WithMessage("El código del síntoma es requerido.")
            .MaximumLength(20).WithMessage("El código no debe exceder 20 caracteres.");

        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre del síntoma es requerido.")
            .MaximumLength(120).WithMessage("El nombre no debe exceder 120 caracteres.");

        RuleFor(x => x.NivelGravedadBase)
            .InclusiveBetween(1, 5).WithMessage("El nivel de gravedad debe estar entre 1 (mínimo) y 5 (máximo).");
    }
}
