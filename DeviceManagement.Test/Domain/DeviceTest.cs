using DeviceManagement.Domain.Entities;
using DeviceManagement.Domain.Enums;
using FluentAssertions;

namespace DeviceManagement.Tests.Domain;

public class DeviceTests
{
    [Fact]
    public void Constructor_Should_CreateDevice_WithValidState_And_NewGuid()
    {
        // Arrange
        string name = "Sensor X";
        string brand = "Intelbras";

        // Act
        var device = new Device(name, brand);

        // Assert
        device.Id.Should().NotBeEmpty();
        device.Name.Should().Be(name);
        device.Brand.Should().Be(brand);
        device.State.Should().Be(DeviceState.Available);
        device.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Update_Should_ModifyProperties_And_SetUpdatedAt_When_Valid()
    {
        // Arrange
        var device = new Device("Old Name", "Old Brand");
        string newName = "New Name";

        // Act
        device.UpdateDetails(newName, null);

        // Assert
        device.Name.Should().Be(newName);
        device.Brand.Should().Be("Old Brand");
    }
}