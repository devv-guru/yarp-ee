using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YarpEe.Domain.Entities;

namespace YarpEe.Adapters.Persistence.Postgres.Configurations;

public class DestinationConfiguration : IEntityTypeConfiguration<Destination>
{
    public void Configure(EntityTypeBuilder<Destination> builder)
    {
        builder.ToTable("destinations");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(d => d.ClusterId)
            .HasColumnName("cluster_id")
            .IsRequired();

        builder.Property(d => d.Address)
            .HasColumnName("address")
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(d => d.HealthPath)
            .HasColumnName("health_path")
            .HasMaxLength(500);
    }
}
