using DiagnosticCenter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiagnosticCenter.Infrastructure.Data.Configurations;

/// <summary>
/// Entity configuration for TestSetup
/// </summary>
public class TestSetupConfiguration : IEntityTypeConfiguration<TestSetup>
{
    public void Configure(EntityTypeBuilder<TestSetup> builder)
    {
        builder.ToTable("test_setup");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("Id");

        builder.Property(e => e.TestTypeId)
            .IsRequired()
            .HasColumnName("TestTypeId");

        builder.Property(e => e.TestName)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("TestName");

        builder.Property(e => e.TestFee)
            .IsRequired()
            .HasColumnType("decimal(18,2)")
            .HasColumnName("TestFee");

        builder.Property(e => e.Description)
            .HasMaxLength(500)
            .HasColumnName("Description");

        builder.Property(e => e.CreatedDate)
            .IsRequired()
            .HasColumnName("created_at")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(e => e.ModifiedDate)
            .HasColumnName("modified_at");

        builder.Property(e => e.IsActive)
            .IsRequired()
            .HasColumnName("IsActive")
            .HasDefaultValue(true);

        builder.Property(e => e.CreatedBy)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("CreatedBy");

        builder.Property(e => e.ModifiedBy)
            .HasMaxLength(100)
            .HasColumnName("ModifiedBy");

        builder.HasMany(e => e.TestEntries)
            .WithOne(e => e.Test)
            .HasForeignKey(e => e.TestId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => e.TestTypeId);
        builder.HasIndex(e => e.TestName);
    }
}
