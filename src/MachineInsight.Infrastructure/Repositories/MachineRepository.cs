using MachineInsight.Domain.Entities;
using MachineInsight.Domain.Enums;
using MachineInsight.Domain.Interfaces;
using MachineInsight.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MachineInsight.Infrastructure.Repositories;

public class MachineRepository : IMachineRepository
{
    private readonly MachineInsightDbContext _context;
    private readonly ILogger<MachineRepository> _logger;

    public MachineRepository(
        MachineInsightDbContext context,
        ILogger<MachineRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Machine>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("GetAllAsync: Retrieving all machines");
        var machines = await _context.Machines
                                     .AsNoTracking()
                                     .ToListAsync(cancellationToken);
        _logger.LogInformation("GetAllAsync: Retrieved {Count} machines", machines.Count);
        return machines;
    }

    public async Task<Machine?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("GetByIdAsync: Retrieving machine with ID {MachineId}", id);
        var machine = await _context.Machines
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        if (machine is null)
            _logger.LogWarning("GetByIdAsync: Machine with ID {MachineId} not found", id);
        else
            _logger.LogInformation("GetByIdAsync: Found machine with ID {MachineId}", id);
        return machine;
    }

    public async Task<IEnumerable<Machine>> GetByStatusAsync(MachineStatus status, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("GetByStatusAsync: Retrieving machines with status {Status}", status);
        var machines = await _context.Machines
                                     .AsNoTracking()
                                     .Where(m => m.Status == status)
                                     .ToListAsync(cancellationToken);
        _logger.LogInformation("GetByStatusAsync: Retrieved {Count} machines with status {Status}", machines.Count, status);
        return machines;
    }

    public async Task AddAsync(Machine machine, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "AddAsync: Adding machine {MachineId} [Name={Name}, Lat={Lat}, Lon={Lon}, Status={Status}, Rpm={Rpm}]",
            machine.Id, machine.Name,
            machine.Location.Latitude, machine.Location.Longitude,
            machine.Status, machine.Rpm);

        await _context.Machines.AddAsync(machine, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("AddAsync: Machine {MachineId} added successfully", machine.Id);
    }

    public async Task UpdateAsync(Machine machine, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "UpdateAsync: Updating machine {MachineId} to [Status={Status}, Rpm={Rpm}]",
            machine.Id, machine.Status, machine.Rpm);

        _context.Machines.Update(machine);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("UpdateAsync: Machine {MachineId} updated successfully", machine.Id);
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("ExistsByNameAsync: Checking if machine with Name={Name} exists", name);
        var exists = await _context.Machines
            .AsNoTracking()
            .AnyAsync(m => m.Name.ToLower() == name.ToLower(), cancellationToken);
        _logger.LogInformation("ExistsByNameAsync: Name={Name} exists: {Exists}", name, exists);
        return exists;
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("ExistsAsync: Checking if machine {MachineId} exists", id);
        var exists = await _context.Machines.AnyAsync(m => m.Id == id, cancellationToken);
        _logger.LogInformation("ExistsAsync: Machine {MachineId} exists: {Exists}", id, exists);
        return exists;
    }

    public async Task DeleteAsync(Machine machine, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("DeleteAsync: Deleting machine {MachineId}", machine.Id);

        _context.Machines.Remove(machine);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("DeleteAsync: Machine {MachineId} deleted successfully", machine.Id);
    }
}
