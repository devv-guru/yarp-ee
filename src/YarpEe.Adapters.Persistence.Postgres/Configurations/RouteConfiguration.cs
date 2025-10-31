using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YarpEe.Domain.Entities;
using YarpEe.Domain.ValueObjects;

namespace YarpEe.Adapters.Persistence.Postgres.Configurations;

public class RouteConfiguration : IEntityTypeConfiguration<Route>
{
    public void Configure(EntityTypeBuilder<Route> builder)
    {
        builder.ToTable("routes");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(r => r.HostId)
            .HasColumnName("host_id")
            .IsRequired();

        builder.Property(r => r.ClusterId)
            .HasColumnName("cluster_id")
            .IsRequired();

        builder.Property(r => r.Path)
            .HasColumnName("path")
            .HasConversion(
                v => v.Value,
                v => new RoutePath(v))
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(r => r.Order)
            .HasColumnName("order")
            .IsRequired();

        builder.Property(r => r.Methods)
            .HasColumnName("methods")
            .HasConversion(
                v => string.Join(",", v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());

        builder.Property(r => r.Enabled)
            .HasColumnName("enabled")
            .IsRequired();

        builder.HasIndex(r => new { r.HostId, r.Path });
    }
}
