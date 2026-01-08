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

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasColumnName("Id");

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("Name");

        builder.Property(t => t.Fee)
            .IsRequired()
            .HasPrecision(18, 2)
            .HasColumnName("Fee");

        builder.Property(t => t.TestTypeId)
            .IsRequired()
            .HasColumnName("TypeId");

        builder.Property(t => t.CreatedDate)
            .IsRequired()
            .HasColumnName("created_at")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(t => t.ModifiedDate)
            .HasColumnName("modified_at");

        builder.Property(t => t.IsActive)
            .IsRequired()
            .HasDefaultValue(true)
            .HasColumnName("IsActive");

        builder.Property(t => t.CreatedBy)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("CreatedBy");

        builder.Property(t => t.ModifiedBy)
            .HasMaxLength(100)
            .HasColumnName("ModifiedBy");

        builder.HasOne(t => t.TestType)
            .WithMany(tt => tt.TestSetups)
            .HasForeignKey(t => t.TestTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(t => t.TestEntries)
            .WithOne(te => te.TestSetup)
            .HasForeignKey(te => te.TestSetupId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(t => t.Name).IsUnique();
    }
}
