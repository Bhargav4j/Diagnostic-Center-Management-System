using DiagnosticCenter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiagnosticCenter.Infrastructure.Data.Configurations;

public class TestSetupConfiguration : IEntityTypeConfiguration<TestSetup>
{
    public void Configure(EntityTypeBuilder<TestSetup> builder)
    {
        builder.ToTable("test_setups", "public");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnType("varchar(200)");

        builder.Property(t => t.Fee)
            .IsRequired()
            .HasColumnType("numeric(18,2)");

        builder.Property(t => t.TestTypeId)
            .IsRequired();

        builder.Property(t => t.CreatedBy)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnType("varchar(100)");

        builder.Property(t => t.ModifiedBy)
            .HasMaxLength(100)
            .HasColumnType("varchar(100)");

        builder.Property(t => t.CreatedDate)
            .IsRequired()
            .HasColumnType("timestamp without time zone");

        builder.Property(t => t.ModifiedDate)
            .HasColumnType("timestamp without time zone");

        builder.Property(t => t.IsActive)
            .IsRequired()
            .HasDefaultValue(true)
            .HasColumnType("boolean");

        builder.HasMany(t => t.PatientTests)
            .WithOne(pt => pt.TestSetup)
            .HasForeignKey(pt => pt.TestSetupId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
