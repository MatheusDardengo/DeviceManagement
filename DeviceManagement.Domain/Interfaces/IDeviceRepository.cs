using DeviceManagement.Domain.Entities;

namespace DeviceManagement.Domain.Interfaces;

public interface IDeviceRepository
{
    Task<Device?> GetByIdAsync(Guid id);
    Task<IEnumerable<Device>> GetAllAsync(string? brand = null, int? state = null);
    Task AddAsync(Device device);
    Task UpdateAsync(Device device);
    Task DeleteAsync(Device device);
    Task SaveChangesAsync();
}
