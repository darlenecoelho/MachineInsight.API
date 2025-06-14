using FluentAssertions;
using MachineInsight.Domain.Entities;
using MachineInsight.Domain.Enums;
using MachineInsight.Domain.ValueObjects;
using Xunit;

namespace MachineInsight.Tests.Entities;

public class MachineTests : TestConfig<Machine>
{
    [Fact]
    public void Constructor_ValidArguments_ShouldInitializeAllPropertiesCorrectly()
    {
        // Arrange
        var name = _faker.Commerce.ProductName();
        var latitude = _faker.Random.Double(-90, 90);
        var longitude = _faker.Random.Double(-180, 180);
        var status = MachineStatus.Operating;
        var location = new Location(latitude, longitude);
        var before = DateTime.UtcNow;

        // Act
        var machine = new Machine(name, location, status);
        var after = DateTime.UtcNow;

        // Assert
        machine.Id.Should().NotBeEmpty();
        machine.Name.Should().Be(name);
        machine.Location.Should().BeEquivalentTo(location);
        machine.Status.Should().Be(status);
        machine.Rpm.Should().Be(0);
        machine.CreatedAt.Should()
               .BeOnOrAfter(before)
               .And.BeOnOrBefore(after);
        machine.UpdatedAt.Should()
               .BeOnOrAfter(before)
               .And.BeOnOrBefore(after);
    }

    [Fact]
    public void UpdateDetails_ValidArguments_ShouldChangeNameLocationAndUpdateTimestamp()
    {
        // Arrange
        var initial = new Machine(
            _faker.Commerce.ProductName(),
            new Location(0, 0),
            MachineStatus.Operating);

        var oldUpdated = initial.UpdatedAt;
        Thread.Sleep(10);

        var newName = _faker.Commerce.ProductName();
        var newLoc = new Location(
            _faker.Random.Double(-90, 90),
            _faker.Random.Double(-180, 180));

        // Act
        initial.UpdateDetails(newName, newLoc);

        // Assert
        initial.Name.Should().Be(newName);
        initial.Location.Should().BeEquivalentTo(newLoc);
        initial.UpdatedAt.Should().BeAfter(oldUpdated);
        initial.CreatedAt.Should().BeBefore(initial.UpdatedAt);
    }

    [Fact]
    public void UpdateTelemetry_ValidArguments_ShouldChangeStatusAndRpmOnly()
    {
        // Arrange
        var machine = new Machine(
            _faker.Commerce.ProductName(),
            new Location(1, 1),
            MachineStatus.Shutdown);

        var oldCreated = machine.CreatedAt;
        var oldUpdated = machine.UpdatedAt;

        var newStatus = MachineStatus.Maintenance;
        var newRpm = _faker.Random.Int(500, 3000);

        // Act
        machine.UpdateTelemetry(newStatus, newRpm);

        // Assert
        machine.Status.Should().Be(newStatus);
        machine.Rpm.Should().Be(newRpm);
        machine.CreatedAt.Should().Be(oldCreated);
        machine.UpdatedAt.Should().Be(oldUpdated,
            "UpdateTelemetry not change UpdatedAt");
    }
}
