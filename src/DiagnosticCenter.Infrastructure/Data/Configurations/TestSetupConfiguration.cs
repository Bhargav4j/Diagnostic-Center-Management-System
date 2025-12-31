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
            .HasColumnName("id");

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("name");

        builder.Property(t => t.Fee)
            .HasColumnType("decimal(18,2)")
            .HasColumnName("fee");

        builder.Property(t => t.TypeId)
            .HasColumnName("type_id");

        builder.Property(t => t.CreatedDate)
            .HasColumnName("created_date")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(t => t.ModifiedDate)
            .HasColumnName("modified_date");

        builder.Property(t => t.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        builder.Property(t => t.CreatedBy)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("created_by");

        builder.Property(t => t.ModifiedBy)
            .HasMaxLength(100)
            .HasColumnName("modified_by");

        builder.HasOne(t => t.TestType)
            .WithMany(tt => tt.TestSetups)
            .HasForeignKey(t => t.TypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(t => t.TestEntries)
            .WithOne(te => te.Test)
            .HasForeignKey(te => te.TestId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
