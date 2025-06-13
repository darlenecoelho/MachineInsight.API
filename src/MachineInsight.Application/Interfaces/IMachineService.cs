using MachineInsight.Application.DTOs;
using MachineInsight.Domain.Enums;

namespace MachineInsight.Application.Interfaces;

public interface IMachineService
{
    Task<CreateMachineResponse> CreateAsync(CreateMachineDto dto);
    Task<IEnumerable<ResponseMachineDto>> GetAllAsync();
    Task<IEnumerable<ResponseMachineDto>> GetByStatusAsync(MachineStatus status);
    Task<ResponseMachineDto?> GetByIdAsync(Guid id);
    Task UpdateTelemetryAsync(Guid id, UpdateTelemetryDto dto);
    Task<ResponseMachineDto?> UpdateAsync(Guid id, UpdateMachineDto dto);
    Task<bool> DeleteAsync(Guid id);
}
