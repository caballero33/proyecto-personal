using FluentValidation;
using TriageDoc.Application.Features.Pacientes.DTOs;

namespace TriageDoc.Application.Features.Pacientes.Validators;

public class CreatePacienteValidator : AbstractValidator<CreatePacienteRequestDto>
{
    public CreatePacienteValidator()
    {
        RuleFor(x => x.Dni)
            .NotEmpty().WithMessage("El documento de identidad (DNI) es obligatorio.")
            .MaximumLength(20).WithMessage("El DNI no puede exceder 20 caracteres.");

        RuleFor(x => x.NombreCompleto)
            .NotEmpty().WithMessage("El nombre completo del paciente es obligatorio.")
            .MinimumLength(3).WithMessage("El nombre debe tener al menos 3 caracteres.")
            .MaximumLength(150).WithMessage("El nombre no puede exceder 150 caracteres.");

        RuleFor(x => x.Edad)
            .InclusiveBetween(0, 125).WithMessage("La edad debe ser un valor válido entre 0 y 125 años.");
    }
}

public class UpdatePacienteValidator : AbstractValidator<UpdatePacienteRequestDto>
{
    public UpdatePacienteValidator()
    {
        RuleFor(x => x.Dni)
            .NotEmpty().WithMessage("El documento de identidad (DNI) es obligatorio.")
            .MaximumLength(20).WithMessage("El DNI no puede exceder 20 caracteres.");

        RuleFor(x => x.NombreCompleto)
            .NotEmpty().WithMessage("El nombre completo del paciente es obligatorio.")
            .MinimumLength(3).WithMessage("El nombre debe tener al menos 3 caracteres.")
            .MaximumLength(150).WithMessage("El nombre no puede exceder 150 caracteres.");

        RuleFor(x => x.Edad)
            .InclusiveBetween(0, 125).WithMessage("La edad debe ser un valor válido entre 0 y 125 años.");
    }
}
