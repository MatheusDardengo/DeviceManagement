using FluentValidation;

namespace DeviceManagement.Application.UseCases.CreateDevice;

public class CreateDeviceRequestValidator : AbstractValidator<CreateDeviceRequest>
{
    public CreateDeviceRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Device name is required.")
            .MaximumLength(100).WithMessage("Device name cannot exceed 100 characters.");

        RuleFor(x => x.Brand)
            .NotEmpty().WithMessage("Device brand is required.")
            .MaximumLength(50).WithMessage("Device brand cannot exceed 50 characters.");
    }
}