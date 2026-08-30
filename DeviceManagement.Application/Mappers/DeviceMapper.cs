using DeviceManagement.Application.UseCases.GetByIdDevice;
using DeviceManagement.Domain.Entities;

namespace DeviceManagement.Application.Mappers;

public static class DeviceMapper
{
    public static GetByIdDeviceResponse ToResponse(this Device device)
    {
        if (device == null)
            return null!;

        return new GetByIdDeviceResponse
        {
            Id = device.Id,
            Name = device.Name,
            Brand = device.Brand,
            State = device.State,
            CreatedAt = device.CreatedAt,
            UpdatedAt = device.UpdatedAt
        };
    }

    public static IEnumerable<GetByIdDeviceResponse> ToResponseList(this IEnumerable<Device> devices)
    { 
        return devices.Select(device => device.ToResponse());
    }
}