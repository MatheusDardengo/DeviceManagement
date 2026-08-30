using DeviceManagement.Application.Services;
using DeviceManagement.Application.UseCases.CreateDevice;
using DeviceManagement.Application.UseCases.DeleteDevice;
using DeviceManagement.Application.UseCases.GetAllDevice;
using DeviceManagement.Application.UseCases.GetByIdDevice;
using DeviceManagement.Application.UseCases.UpdateDevice;
using DeviceManagement.Domain.Entities;
using DeviceManagement.Domain.Enums;
using DeviceManagement.Domain.Interfaces;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace DeviceManagement.Tests.Application;

public class DeviceServiceTests
{
    private readonly Mock<IDeviceRepository> _repositoryMock;
    private readonly DeviceService _service;

    public DeviceServiceTests()
    {
        _repositoryMock = new Mock<IDeviceRepository>();

        _service = new DeviceService(_repositoryMock.Object);
    }

    // -------------------------------------------------------------------------
    // CreateDeviceAsync
    // -------------------------------------------------------------------------

    [Fact]
    public async Task CreateDeviceAsync_Should_ReturnDevice_When_RequestIsValid()
    {
        // Arrange
        var request = new CreateDeviceRequest { Name = "DeviceTest", Brand = "Apple" };

        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Device>()));
        _repositoryMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        // Act
        Device result = await _service.CreateDeviceAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("DeviceTest");
        result.Brand.Should().Be("Apple");
        result.State.Should().Be(DeviceState.Available);

        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Device>()), Times.Once);
    }

    // -------------------------------------------------------------------------
    // GetByIdDeviceAsync
    // -------------------------------------------------------------------------

    [Fact]
    public async Task GetByIdDeviceAsync_Should_ReturnResponse_When_DeviceExists()
    {
        // Arrange
        var device = new Device("Monitor", "Samsung");
        var request = new GetByIdDeviceRequest { Id = device.Id };

        _repositoryMock.Setup(r => r.GetByIdAsync(device.Id)).ReturnsAsync(device);

        // Act
        GetByIdDeviceResponse result = await _service.GetByIdDeviceAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(device.Id);
        result.Name.Should().Be("Monitor");
        result.Brand.Should().Be("Samsung");
    }

    [Fact]
    public async Task GetByIdDeviceAsync_Should_ThrowKeyNotFoundException_When_DeviceDoesNotExist()
    {
        // Arrange
        var request = new GetByIdDeviceRequest { Id = Guid.NewGuid() };

        _repositoryMock.Setup(r => r.GetByIdAsync(request.Id)).ReturnsAsync((Device?)null);

        // Act
        Func<Task> act = async () => await _service.GetByIdDeviceAsync(request);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Device with ID {request.Id} not found.");
    }

    // -------------------------------------------------------------------------
    // GetAllDevicesAsync
    // -------------------------------------------------------------------------

    [Fact]
    public async Task GetAllDevicesAsync_Should_ReturnAllDevices_When_NoFilterApplied()
    {
        // Arrange
        var request = new GetAllDevicesRequest();
        var devices = new List<Device> { new("Phone", "Apple"), new("Tablet", "Samsung") };

        _repositoryMock.Setup(r => r.GetAllAsync(null, null)).ReturnsAsync(devices);

        // Act
        GetAllDevicesResponse result = await _service.GetAllDevicesAsync(request);

        // Assert
        result.Devices.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetAllDevicesAsync_Should_ReturnEmpty_When_NoDevicesFound()
    {
        // Arrange
        var request = new GetAllDevicesRequest { Brand = "Unknown" };

        _repositoryMock.Setup(r => r.GetAllAsync("Unknown", null)).ReturnsAsync([]);

        // Act
        GetAllDevicesResponse result = await _service.GetAllDevicesAsync(request);

        // Assert
        result.Devices.Should().BeEmpty();
    }

    // -------------------------------------------------------------------------
    // UpdateDeviceAsync
    // -------------------------------------------------------------------------

    [Fact]
    public async Task UpdateDeviceAsync_Should_ReturnUpdatedResponse_When_DeviceExists()
    {
        // Arrange
        var device = new Device("OldName", "OldBrand");
        var request = new UpdateDeviceRequest { Id = device.Id, Name = "NewName", Brand = "NewBrand" };

        _repositoryMock.Setup(r => r.GetByIdAsync(device.Id)).ReturnsAsync(device);
        _repositoryMock.Setup(r => r.UpdateAsync(device)).Returns(Task.CompletedTask);
        _repositoryMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        // Act
        GetByIdDeviceResponse result = await _service.UpdateDeviceAsync(request);

        // Assert
        result.Name.Should().Be("NewName");
        result.Brand.Should().Be("NewBrand");
        _repositoryMock.Verify(r => r.UpdateAsync(device), Times.Once);
    }

    [Fact]
    public async Task UpdateDeviceAsync_Should_ThrowKeyNotFoundException_When_DeviceDoesNotExist()
    {
        // Arrange
        var request = new UpdateDeviceRequest { Id = Guid.NewGuid(), Name = "Name", Brand = "Brand" };

        _repositoryMock.Setup(r => r.GetByIdAsync(request.Id)).ReturnsAsync((Device?)null);

        // Act
        Func<Task> act = async () => await _service.UpdateDeviceAsync(request);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Device with ID {request.Id} not found.");
    }

    [Fact]
    public async Task UpdateDeviceAsync_Should_ThrowInvalidOperationException_When_DeviceIsInUse()
    {
        // Arrange
        var device = new Device("Phone", "Apple");
        device.UpdateState(DeviceState.InUse);

        var request = new UpdateDeviceRequest { Id = device.Id, Name = "NewName", Brand = "NewBrand" };

        _repositoryMock.Setup(r => r.GetByIdAsync(device.Id)).ReturnsAsync(device);

        // Act
        Func<Task> act = async () => await _service.UpdateDeviceAsync(request);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Name and brand cannot be updated when the device is in use.");
    }

    // -------------------------------------------------------------------------
    // DeleteDeviceAsync
    // -------------------------------------------------------------------------

    [Fact]
    public async Task DeleteDeviceAsync_Should_ThrowKeyNotFoundException_When_DeviceDoesNotExist()
    {
        // Arrange
        var request = new DeleteDeviceRequest { Id = Guid.NewGuid() };

        _repositoryMock.Setup(r => r.GetByIdAsync(request.Id)).ReturnsAsync((Device?)null);

        // Act
        Func<Task> act = async () => await _service.DeleteDeviceAsync(request);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Device with ID {request.Id} not found.");

        _repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Device>()), Times.Never);
    }

    [Fact]
    public async Task DeleteDeviceAsync_Should_CallDelete_When_DeviceExists()
    {
        // Arrange
        var device = new Device("Test", "Brand");
        var request = new DeleteDeviceRequest { Id = device.Id };

        _repositoryMock.Setup(r => r.GetByIdAsync(request.Id)).ReturnsAsync(device);
        _repositoryMock.Setup(r => r.DeleteAsync(device)).Returns(Task.CompletedTask);

        // Act
        await _service.DeleteDeviceAsync(request);

        // Assert
        _repositoryMock.Verify(r => r.DeleteAsync(device), Times.Once);
    }
}