using DeviceManagement.Domain.Enums;

namespace DeviceManagement.Domain.Entities;

public class Device
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Brand { get; private set; }
    public DeviceState State { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Device(string name, string brand)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));

        if (string.IsNullOrWhiteSpace(brand))
            throw new ArgumentException("Brand cannot be null or empty.", nameof(brand));

        Id = Guid.NewGuid();
        Name = name;
        Brand = brand;
        State = DeviceState.Available;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(string? name, string? brand)
    {
        if (State == DeviceState.InUse)
        {
            throw new InvalidOperationException("Name and brand cannot be updated when the device is in use.");
        }

        //TODO review those rules
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));

        if (string.IsNullOrWhiteSpace(brand))
            throw new ArgumentException("Brand cannot be null or empty.", nameof(brand));

        Name = name;
        Brand = brand;
    }

    public void UpdateState(DeviceState newState)
    {
        State = newState;
    }

    public void ValidateDeletion()
    {
        if (State == DeviceState.InUse)
        {
            throw new InvalidOperationException("In-use devices cannot be deleted.");
        }
    }


}
