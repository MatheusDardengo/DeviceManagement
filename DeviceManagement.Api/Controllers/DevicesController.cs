using DeviceManagement.Application.Interfaces;
using DeviceManagement.Application.UseCases.CreateDevice;
using DeviceManagement.Application.UseCases.DeleteDevice;
using DeviceManagement.Application.UseCases.GetAllDevice;
using DeviceManagement.Application.UseCases.GetByIdDevice;
using DeviceManagement.Application.UseCases.UpdateDevice;
using DeviceManagement.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManagement.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class DevicesController(IDeviceService deviceService) : ControllerBase
{
    /// <summary>
    /// Create a new device.
    /// </summary>
    /// <param name="request">The request containing the details of the device to be created.</param>
    /// <returns>The newly created device.</returns>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDeviceRequest request)
    {
        Device device = await deviceService.CreateDeviceAsync(request);

        return CreatedAtAction(nameof(Create), new { id = device.Id }, device);
    }

    /// <summary>
    /// Get a device by its unique identifier.
    /// </summary>
    /// <param name="request">The request containing the unique identifier of the device.</param>
    /// <returns>The device matching the unique identifier.</returns>
    [HttpGet]
    public async Task<IActionResult> GetById([FromQuery] GetByIdDeviceRequest request)
    {
        GetByIdDeviceResponse response = await deviceService.GetByIdDeviceAsync(request);

        return Ok(response);
    }

    /// <summary>
    /// Get all devices with optional filtering by Brand and State.
    /// </summary>
    /// <param name="request">The request containing optional filtering parameters.</param>
    /// <returns>A list of devices matching the filtering criteria.</returns>
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll([FromQuery] GetAllDevicesRequest request)
    {
        GetAllDevicesResponse response = await deviceService.GetAllDevicesAsync(request);

        return Ok(response);
    }

    /// <summary>
    /// Deletes a device by its unique identifier.
    /// </summary>
    /// <param name="request">The request containing the unique identifier of the device to be deleted.</param>
    /// <returns>No content if the deletion is successful.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] DeleteDeviceRequest request)
    {
        await deviceService.DeleteDeviceAsync(request);

        return NoContent();
    }

    /// <summary>
    /// Updates an existing device by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the device to be updated.</param>
    /// <param name="request">The request containing the updated details of the device.</param>
    /// <returns>The updated device.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDeviceRequest request)
    {
        request.Id = id;

        var updatedDevice = await deviceService.UpdateDeviceAsync(request);

        return Ok(updatedDevice);
    }

}
