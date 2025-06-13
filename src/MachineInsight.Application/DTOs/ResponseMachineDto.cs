using MachineInsight.Domain.Enums;

namespace MachineInsight.Application.DTOs;

public class ResponseMachineDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public MachineStatus Status { get; set; }
    public int Rpm { get; set; }
    public DateTime CreatedAt { get; set; }
}
