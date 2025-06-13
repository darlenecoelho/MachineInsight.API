namespace MachineInsight.Application.DTOs;

public class CreateMachineResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public ResponseMachineDto? Data { get; set; }
}
