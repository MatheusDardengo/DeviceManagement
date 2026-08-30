using DeviceManagement.Domain.Enums;

namespace DeviceManagement.Application.UseCases.UpdateDevice;

public class UpdateDeviceResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public DeviceState State { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
