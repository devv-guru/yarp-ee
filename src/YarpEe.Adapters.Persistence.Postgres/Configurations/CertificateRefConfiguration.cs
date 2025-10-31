using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YarpEe.Domain.Entities;
using YarpEe.Domain.ValueObjects;

namespace YarpEe.Adapters.Persistence.Postgres.Configurations;

public class CertificateRefConfiguration : IEntityTypeConfiguration<CertificateRef>
{
    public void Configure(EntityTypeBuilder<CertificateRef> builder)
    {
        builder.ToTable("certificates");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(c => c.Kind)
            .HasColumnName("kind")
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.Location)
            .HasColumnName("location")
            .HasConversion(
                v => v.Value,
                v => new CertificateLocation(v))
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(c => c.PasswordSecret)
            .HasColumnName("password_secret")
            .HasMaxLength(200);
    }
}
