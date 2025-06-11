using FluentValidation;
using MachineInsight.Application.DTOs;

namespace MachineInsight.Application.Validators;

public class CreateMachineDtoValidator : AbstractValidator<CreateMachineDto>
{
    public CreateMachineDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome da máquina é obrigatório.")
            .MaximumLength(100).WithMessage("O nome deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90).WithMessage("Latitude inválida.");

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180).WithMessage("Longitude inválida.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Status inválido.");
    }
}