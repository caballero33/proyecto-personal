using FluentValidation;
using TriageDoc.Application.Features.Protocolos.DTOs;

namespace TriageDoc.Application.Features.Protocolos.Validators;

public class CreateProtocoloValidator : AbstractValidator<CreateProtocoloRequestDto>
{
    public CreateProtocoloValidator()
    {
        RuleFor(x => x.CodigoColor)
            .NotEmpty().WithMessage("El código de color Manchester es requerido (Rojo, Naranja, Amarillo, Verde, Azul).")
            .MaximumLength(30);

        RuleFor(x => x.NombreNivel)
            .NotEmpty().WithMessage("El nombre del nivel de triage es requerido.")
            .MaximumLength(50);

        RuleFor(x => x.TiempoMaximoEsperaMinutos)
            .GreaterThanOrEqualTo(0).WithMessage("El tiempo de espera debe ser igual o superior a 0 minutos.");

        RuleFor(x => x.AreaDerivacion)
            .NotEmpty().WithMessage("El área clínica de derivación es requerida.")
            .MaximumLength(120);
    }
}

public class UpdateProtocoloValidator : AbstractValidator<UpdateProtocoloRequestDto>
{
    public UpdateProtocoloValidator()
    {
        RuleFor(x => x.CodigoColor)
            .NotEmpty().WithMessage("El código de color Manchester es requerido.")
            .MaximumLength(30);

        RuleFor(x => x.NombreNivel)
            .NotEmpty().WithMessage("El nombre del nivel de triage es requerido.")
            .MaximumLength(50);

        RuleFor(x => x.TiempoMaximoEsperaMinutos)
            .GreaterThanOrEqualTo(0).WithMessage("El tiempo de espera debe ser igual o superior a 0 minutos.");

        RuleFor(x => x.AreaDerivacion)
            .NotEmpty().WithMessage("El área clínica de derivación es requerida.")
            .MaximumLength(120);
    }
}
