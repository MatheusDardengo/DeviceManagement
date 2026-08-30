using FluentValidation;

namespace DeviceManagement.Application.UseCases.GetByIdDevice;

public class GetByIdDeviceValidator : AbstractValidator<GetByIdDeviceRequest>
{
    public GetByIdDeviceValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Device ID is required.");
    }
}
