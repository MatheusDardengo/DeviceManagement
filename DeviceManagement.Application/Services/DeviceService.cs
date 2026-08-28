using DeviceManagement.Application.Interfaces;
using DeviceManagement.Application.UseCases.CreateDevice;
using DeviceManagement.Application.UseCases.DeleteDevice;
using DeviceManagement.Application.UseCases.GetAllDevice;
using DeviceManagement.Application.UseCases.GetByIdDevice;
using DeviceManagement.Application.UseCases.UpdateDevice;
using DeviceManagement.Domain.Entities;
using DeviceManagement.Domain.Interfaces;
using FluentValidation;
using FluentValidation.Results;

namespace DeviceManagement.Application.Services;

public class DeviceService : IDeviceService
{
    private readonly IDeviceRepository _repository;

    //TODO trocar N validators
    private readonly IValidator<CreateDeviceRequest> _createValidator;
    private readonly IValidator<GetByIdDeviceRequest> _getByIdValidator;
    private readonly IValidator<UpdateDeviceRequest> _updateValidator;
    private readonly IValidator<GetAllDevicesRequest> _getAllValidator;

    public DeviceService(
        IDeviceRepository repository,
        IValidator<CreateDeviceRequest> createValidator,
        IValidator<GetByIdDeviceRequest> getByIdValidator,
        IValidator<UpdateDeviceRequest> updateValidator,
        IValidator<GetAllDevicesRequest> getAllValidator)
    {
        _repository = repository;
        _createValidator = createValidator;
        _getByIdValidator = getByIdValidator;
        _getAllValidator = getAllValidator;
        _updateValidator = updateValidator;
    }

    public async Task<Device> CreateDeviceAsync(CreateDeviceRequest request)
    {
        ValidationResult validationResult = await _createValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            //TODO create custom exception (ValidationException) 
            // capturar por um middleware global para retornar um HTTP 400.
            throw new ValidationException(validationResult.Errors);
        }

        var device = new Device(request.Name, request.Brand);

        //TODO rever esse duplo await
        await _repository.AddAsync(device);
        await _repository.SaveChangesAsync();

        return device;
    }

    public async Task<GetAllDevicesResponse> GetAllDevicesAsync(GetAllDevicesRequest request)
    {
        ValidationResult validationResult = await _getAllValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            //TODO create custom exception (ValidationException) 
            // capturar por um middleware global para retornar um HTTP 400.
            throw new ValidationException(validationResult.Errors);
        }

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
        ValidationResult validationResult = await _getByIdValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            //TODO criar custom exception (ValidationException) 
            // capturar por um middleware global para retornar um HTTP 400.
            throw new ValidationException(validationResult.Errors);
        }

        //TODO criar custom exception (NotFoundException) 
        // capturar por um middleware global para retornar um HTTP 404.
        var device = await _repository.GetByIdAsync(request.Id) ?? throw new KeyNotFoundException($"Device with ID {request.Id} not found.");

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
        ValidationResult validationResult = await _updateValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var device = await _repository.GetByIdAsync(request.Id);
        if (device == null)
        {
            throw new KeyNotFoundException($"Device with ID {request.Id} not found.");
        }

        device.UpdateDetails(request.Name, request.Brand);

        await _repository.UpdateAsync(device);
        await _repository.SaveChangesAsync();

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
        var device = await _repository.GetByIdAsync(request.Id);

        if (device == null)
        {
            throw new KeyNotFoundException($"Device with ID {request.Id} not found.");
        }

        await _repository.DeleteAsync(device);
    }
}
