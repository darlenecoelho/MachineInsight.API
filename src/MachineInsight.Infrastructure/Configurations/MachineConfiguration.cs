using MachineInsight.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MachineInsight.Infrastructure.Configurations;

public class MachineConfiguration : IEntityTypeConfiguration<Machine>
{
    public void Configure(EntityTypeBuilder<Machine> builder)
    {
        builder.ToTable("Machines");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.OwnsOne(m => m.Location, location =>
        {
            location.Property(l => l.Latitude).IsRequired().HasColumnName("Latitude");
            location.Property(l => l.Longitude).IsRequired().HasColumnName("Longitude");
        });

        builder.Property(m => m.Status)
               .IsRequired()
               .HasConversion<int>();

        builder.Property(m => m.RPM)
               .IsRequired();
    }
}
