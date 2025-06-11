using MachineInsight.Domain.Enums;
using MachineInsight.Domain.ValueObjects;

namespace MachineInsight.Domain.Entities;

public class Machine
{
    protected Machine() { }

    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; }
    public Location Location { get; private set; }
    public MachineStatus Status { get; private set; }
    public int RPM { get; private set; }

    public Machine(string name, Location location, MachineStatus status, int rpm = 0)
    {
        Name = name;
        Location = location;
        Status = status;
        RPM = rpm;
    }

    public void UpdateTelemetry(Location location, MachineStatus status, int rpm)
    {
        Location = location;
        Status = status;
        RPM = rpm;
    }
}
