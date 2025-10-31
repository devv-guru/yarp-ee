using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YarpEe.Domain.Entities;
using YarpEe.Domain.ValueObjects;

namespace YarpEe.Adapters.Persistence.Postgres.Configurations;

public class HostConfiguration : IEntityTypeConfiguration<Host>
{
    public void Configure(EntityTypeBuilder<Host> builder)
    {
        builder.ToTable("hosts");

        builder.HasKey(h => h.Id);

        builder.Property(h => h.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(h => h.Name)
            .HasColumnName("name")
            .HasConversion(
                v => v.Value,
                v => new HostName(v))
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(h => h.BaseUrl)
            .HasColumnName("base_url")
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(h => h.CertificateRef)
            .HasColumnName("certificate_ref")
            .HasMaxLength(200);

        builder.Property(h => h.CreatedUtc)
            .HasColumnName("created_utc")
            .IsRequired();

        builder.Property(h => h.UpdatedUtc)
            .HasColumnName("updated_utc")
            .IsRequired();

        builder.HasIndex(h => h.Name)
            .IsUnique();
    }
}
