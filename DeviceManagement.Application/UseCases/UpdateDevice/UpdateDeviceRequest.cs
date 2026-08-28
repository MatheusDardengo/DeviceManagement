namespace DeviceManagement.Application.UseCases.UpdateDevice;

public class UpdateDeviceRequest
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Brand { get; set; }
}