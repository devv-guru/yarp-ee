using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YarpEe.Domain.Entities;

namespace YarpEe.Adapters.Persistence.Postgres.Configurations;

public class ClusterConfiguration : IEntityTypeConfiguration<Cluster>
{
    public void Configure(EntityTypeBuilder<Cluster> builder)
    {
        builder.ToTable("clusters");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(c => c.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.LoadBalancingPolicy)
            .HasColumnName("load_balancing_policy")
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.CreatedUtc)
            .HasColumnName("created_utc")
            .IsRequired();

        builder.Property(c => c.UpdatedUtc)
            .HasColumnName("updated_utc")
            .IsRequired();

        builder.HasIndex(c => c.Name)
            .IsUnique();

        builder.HasMany(c => c.Destinations)
            .WithOne()
            .HasForeignKey(d => d.ClusterId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
