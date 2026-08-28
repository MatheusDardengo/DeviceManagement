using DeviceManagement.Application.UseCases.CreateDevice;
using DeviceManagement.Application.UseCases.DeleteDevice;
using DeviceManagement.Application.UseCases.GetAllDevice;
using DeviceManagement.Application.UseCases.GetByIdDevice;
using DeviceManagement.Application.UseCases.UpdateDevice;
using DeviceManagement.Domain.Entities;

namespace DeviceManagement.Application.Interfaces;

public interface IDeviceService
{
    Task<Device> CreateDeviceAsync(CreateDeviceRequest request);

    Task<GetByIdDeviceResponse> GetByIdDeviceAsync(GetByIdDeviceRequest request);

    Task<GetAllDevicesResponse> GetAllDevicesAsync(GetAllDevicesRequest request);

    Task<GetByIdDeviceResponse> UpdateDeviceAsync(UpdateDeviceRequest request);

    Task DeleteDeviceAsync(DeleteDeviceRequest request);

}
