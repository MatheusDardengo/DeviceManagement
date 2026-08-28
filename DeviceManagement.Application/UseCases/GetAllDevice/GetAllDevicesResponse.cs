using DeviceManagement.Application.UseCases.GetByIdDevice;

namespace DeviceManagement.Application.UseCases.GetAllDevice;

public class GetAllDevicesResponse
{
    public IEnumerable<GetByIdDeviceResponse>? Devices { get; set; }
}
