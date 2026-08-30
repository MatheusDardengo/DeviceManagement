using DeviceManagement.Application.Interfaces;
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
        var device = new Device(request.Name, request.Brand);

        await _repository.AddAsync(device);

        return device;
    }

    public async Task<GetAllDevicesResponse> GetAllDevicesAsync(GetAllDevicesRequest request)
    {
        IEnumerable<Device> devices = await _repository.GetAllAsync(request.Brand, request.State);

        //TODO mudar para mapper (AutoMapper) para mapear a entidade para o DTO de resposta.
        return new GetAllDevicesResponse
        {
            Devices = devices.Select(device => new GetByIdDeviceResponse
            {
                Id = device.Id,
                Name = device.Name,
                Brand = device.Brand,
                State = device.State,
                CreatedAt = device.CreatedAt
            })
        };
    }

    public async Task<GetByIdDeviceResponse> GetByIdDeviceAsync(GetByIdDeviceRequest request)
    {
        //TODO criar custom exception (NotFoundException) 
        // capturar por um middleware global para retornar um HTTP 404.
        Device device = await _repository.GetByIdAsync(request.Id) ?? throw new KeyNotFoundException($"Device with ID {request.Id} not found.");

        //TODO mudar para mapper (AutoMapper) para mapear a entidade para o DTO de resposta.
        return new GetByIdDeviceResponse
        {
            Id = device.Id,
            Name = device.Name,
            Brand = device.Brand,
            State = device.State,
            CreatedAt = device.CreatedAt
        };
    }

    public async Task<GetByIdDeviceResponse> UpdateDeviceAsync(UpdateDeviceRequest request)
    {
        //TODO adicionar validação guid valido.
        //TODO add updatedat
        Device device = await _repository.GetByIdAsync(request.Id) ?? throw new KeyNotFoundException($"Device with ID {request.Id} not found.");

        if (request.State.HasValue)
            device.UpdateState((DeviceState)request.State.Value);

        device.UpdateDetails(request.Name, request.Brand);

        await _repository.UpdateAsync(device);

        //TODO hasdetailschanged para comparar no banco com os dados que mudaram

        //TODO mudar para mapper (AutoMapper) para mapear a entidade para o DTO de resposta.
        return new GetByIdDeviceResponse
        {
            Id = device.Id,
            Name = device.Name,
            Brand = device.Brand,
            State = device.State,
            CreatedAt = device.CreatedAt
        };
    }

    public async Task DeleteDeviceAsync(DeleteDeviceRequest request)
    {
        //TODO adicionar validação guid valido.

        //TODO tratar erro - ver no middleware
        var device = await _repository.GetByIdAsync(request.Id);

        if (device == null)
        {
            throw new KeyNotFoundException($"Device with ID {request.Id} not found.");
        }

        device.ValidateDeletion();

        await _repository.DeleteAsync(device);
    }
}
