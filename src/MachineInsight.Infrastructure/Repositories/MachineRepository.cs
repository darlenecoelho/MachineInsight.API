using MachineInsight.Domain.Entities;
using MachineInsight.Domain.Enums;
using MachineInsight.Domain.Interfaces;
using MachineInsight.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MachineInsight.Infrastructure.Repositories;

public class MachineRepository : IMachineRepository
{
    private readonly MachineInsightDbContext _context;

    public MachineRepository(MachineInsightDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Machine>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Machines.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Machine?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Machines
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Machine>> GetByStatusAsync(MachineStatus status, CancellationToken cancellationToken = default)
    {
        return await _context.Machines
            .AsNoTracking()
            .Where(m => m.Status == status)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Machine machine, CancellationToken cancellationToken = default)
    {
        await _context.Machines.AddAsync(machine, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Machine machine, CancellationToken cancellationToken = default)
    {
        _context.Machines.Update(machine);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Machines.AnyAsync(m => m.Id == id, cancellationToken);
    }
}
