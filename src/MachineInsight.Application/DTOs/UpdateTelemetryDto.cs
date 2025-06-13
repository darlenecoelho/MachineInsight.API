using MachineInsight.Domain.Enums;

namespace MachineInsight.Application.DTOs;

public class UpdateTelemetryDto
{
    public MachineStatus Status { get; set; }
    public int Rpm { get; set; }
}
