using FluentValidation;

namespace DeviceManagement.Application.UseCases.UpdateDevice;

public class UpdateDeviceValidator : AbstractValidator<UpdateDeviceRequest>
{
    public UpdateDeviceValidator()
    {
        When(x => !string.IsNullOrWhiteSpace(x.Name), () =>
        {
            RuleFor(x => x.Name)
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");
        });
        When(x => !string.IsNullOrWhiteSpace(x.Brand), () =>
        {
            RuleFor(x => x.Brand)
                .MaximumLength(50).WithMessage("Brand cannot exceed 50 characters.");
        });
        When(x => x.State.HasValue, () =>
        {
            RuleFor(x => x.State)
                .InclusiveBetween(0, 2).WithMessage("State must be 0 (Inactive), 1 (InUse) or 2 (Inactive).");
        });
    }
}
