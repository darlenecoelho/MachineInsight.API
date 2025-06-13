using MachineInsight.Domain.Enums;
using MachineInsight.Domain.ValueObjects;

namespace MachineInsight.Domain.Entities;

public class Machine
{
    protected Machine() { }

    public Machine(string name, Location location, MachineStatus status)
    {
        Name = name;
        Location = location;
        Status = status;
        Rpm = 0;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; }
    public Location Location { get; private set; }
    public MachineStatus Status { get; private set; }
    public int Rpm { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }


    public void UpdateDetails(string name, Location location)
    {
        Name = name;
        Location = location;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateTelemetry(MachineStatus status, int rpm)
    {
        Status = status;
        Rpm = rpm;
    }
}
