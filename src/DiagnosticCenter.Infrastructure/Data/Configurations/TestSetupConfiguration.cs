using DiagnosticCenter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiagnosticCenter.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for TestSetup entity
/// </summary>
public class TestSetupConfiguration : IEntityTypeConfiguration<TestSetup>
{
    public void Configure(EntityTypeBuilder<TestSetup> builder)
    {
        builder.ToTable("test_setup");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasColumnName("Id");

        builder.Property(t => t.TestName)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("TestName");

        builder.Property(t => t.TestTypeId)
            .IsRequired()
            .HasColumnName("TestTypeId");

        builder.Property(t => t.Fee)
            .IsRequired()
            .HasColumnType("decimal(18,2)")
            .HasColumnName("Fee");

        builder.Property(t => t.Description)
            .HasMaxLength(500)
            .HasColumnName("Description");

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
            .HasColumnName("CreatedBy")
            .HasDefaultValue("System");

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

        builder.HasIndex(t => t.TestName);
        builder.HasIndex(t => t.TestTypeId);
    }
}
