using MachineInsight.Domain.Enums;

namespace MachineInsight.Application.DTOs;

public class TelemetryUpdateDto
{
    public Guid MachineId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public MachineStatus Status { get; set; }
    public int RPM { get; set; }
}
