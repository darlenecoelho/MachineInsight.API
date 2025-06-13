using FluentValidation;
using MachineInsight.Application.DTOs;

namespace MachineInsight.Application.Validators;

public class CreateMachineDtoValidator : AbstractValidator<CreateMachineDto>
{
    public CreateMachineDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Machine name is required.")
            .MaximumLength(100).WithMessage("Machine name must not exceed 100 characters.");

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90.");

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid machine status.");
    }
}