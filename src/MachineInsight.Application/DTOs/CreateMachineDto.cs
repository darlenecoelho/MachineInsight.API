using MachineInsight.Domain.Enums;

namespace MachineInsight.Application.DTOs;

public class CreateMachineDto
{
    public string Name { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public MachineStatus Status { get; set; }
}