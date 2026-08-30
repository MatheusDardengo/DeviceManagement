using DeviceManagement.Domain.Entities;
using DeviceManagement.Domain.Enums;
using DeviceManagement.Domain.Interfaces;
using DeviceManagement.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;


namespace DeviceManagement.Infrastructure.Repositories;

public class DeviceRepository : IDeviceRepository
{
    private readonly ApplicationDbContext _context;

    public DeviceRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Device device)
    {
        _context.Devices.Add(device);
        await SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<Device?> GetByIdAsync(Guid id)
    {
        return await _context.Devices.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<IEnumerable<Device>> GetAllAsync(string? brand = null, int? state = null)
    {
        
        var query = _context.Devices.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(brand))
        {
            query = query.Where(d => d.Brand.Contains(brand));
        }

        if (state.HasValue)
        {
            query = query.Where(d => d.State == (DeviceState) state.Value);
        }

        return await query.ToListAsync();
    }

    public async Task UpdateAsync(Device device)
    {
        _context.Devices.Update(device);
        await SaveChangesAsync();
    }

    public async Task DeleteAsync(Device device)
    {
        _context.Devices.Remove(device);
        await SaveChangesAsync();
    }
}