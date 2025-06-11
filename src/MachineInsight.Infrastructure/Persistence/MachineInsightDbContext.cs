using MachineInsight.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MachineInsight.Infrastructure.Persistence;

public class MachineInsightDbContext : DbContext
{
    public DbSet<Machine> Machines { get; set; }

    public MachineInsightDbContext(DbContextOptions<MachineInsightDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MachineInsightDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
