using MachineInsight.Domain.Entities;
using MachineInsight.Domain.Enums;

namespace MachineInsight.Domain.Interfaces;

public interface IMachineRepository
{
    Task<IEnumerable<Machine>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Machine?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Machine>> GetByStatusAsync(MachineStatus status, CancellationToken cancellationToken = default);
    Task AddAsync(Machine machine, CancellationToken cancellationToken = default);
    Task UpdateAsync(Machine machine, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}
