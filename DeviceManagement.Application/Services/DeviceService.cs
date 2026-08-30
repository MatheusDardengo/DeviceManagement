using DeviceManagement.Application.Interfaces;
using DeviceManagement.Application.Mappers;
using DeviceManagement.Application.UseCases.CreateDevice;
using DeviceManagement.Application.UseCases.DeleteDevice;
using DeviceManagement.Application.UseCases.GetAllDevice;
using DeviceManagement.Application.UseCases.GetByIdDevice;
using DeviceManagement.Application.UseCases.UpdateDevice;
using DeviceManagement.Domain.Entities;
using DeviceManagement.Domain.Enums;
using DeviceManagement.Domain.Interfaces;

namespace DeviceManagement.Application.Services;

public class DeviceService : IDeviceService
{
    private readonly IDeviceRepository _repository;

    public DeviceService(IDeviceRepository repository)
    {
        _repository = repository;
    }

    public async Task<Device> CreateDeviceAsync(CreateDeviceRequest request)
    {
        Device device = new (request.Name, request.Brand);

        await _repository.AddAsync(device);

        return device;
    }

    public async Task<GetAllDevicesResponse> GetAllDevicesAsync(GetAllDevicesRequest request)
    {
        IEnumerable<Device> devices = await _repository.GetAllAsync(request.Brand, request.State);

        return new GetAllDevicesResponse
        {
            Devices = devices.ToResponseList()
        };
    }

    public async Task<GetByIdDeviceResponse> GetByIdDeviceAsync(GetByIdDeviceRequest request)
    {
        Device device = await _repository.GetByIdAsync(request.Id) ?? throw new KeyNotFoundException($"Device with ID {request.Id} not found.");

        return device.ToResponse();

    }

    public async Task<GetByIdDeviceResponse> UpdateDeviceAsync(UpdateDeviceRequest request)
    {
        Device device = await _repository.GetByIdAsync(request.Id) ?? throw new KeyNotFoundException($"Device with ID {request.Id} not found.");

        if (request.State.HasValue)
            device.UpdateState((DeviceState)request.State.Value);

        device.UpdateDetails(request.Name, request.Brand);

        await _repository.UpdateAsync(device);

        return device.ToResponse();
    }

    public async Task DeleteDeviceAsync(DeleteDeviceRequest request)
    {
        var device = await _repository.GetByIdAsync(request.Id);

        if (device == null)
        {
            throw new KeyNotFoundException($"Device with ID {request.Id} not found.");
        }

        device.ValidateDeletion();

        await _repository.DeleteAsync(device);
    }
}
