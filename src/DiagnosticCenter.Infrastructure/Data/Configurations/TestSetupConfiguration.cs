using DiagnosticCenter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiagnosticCenter.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework Core configuration for the TestSetup entity.
/// Defines table mapping, column specifications, and relationships.
/// </summary>
public class TestSetupConfiguration : IEntityTypeConfiguration<TestSetup>
{
    /// <summary>
    /// Configures the TestSetup entity.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<TestSetup> builder)
    {
        // Table configuration
        builder.ToTable("TestSetups");

        // Primary key configuration
        builder.HasKey(ts => ts.Id);
        builder.Property(ts => ts.Id)
            .ValueGeneratedOnAdd()
            .HasComment("Unique identifier for the test setup");

        // Name configuration
        builder.Property(ts => ts.Name)
            .IsRequired()
            .HasMaxLength(200)
            .HasComment("Name of the test");

        builder.HasIndex(ts => ts.Name)
            .HasDatabaseName("IX_TestSetups_Name");

        // Fee configuration
        builder.Property(ts => ts.Fee)
            .IsRequired()
            .HasPrecision(18, 2)
            .HasComment("Fee charged for the test");

        // TypeId configuration (Foreign Key)
        builder.Property(ts => ts.TypeId)
            .IsRequired()
            .HasComment("Foreign key to the test type");

        builder.HasIndex(ts => ts.TypeId)
            .HasDatabaseName("IX_TestSetups_TypeId");

        // Audit fields configuration
        builder.Property(ts => ts.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()")
            .HasComment("Date and time when the test setup was created");

        builder.Property(ts => ts.ModifiedDate)
            .HasComment("Date and time when the test setup was last modified");

        builder.Property(ts => ts.IsActive)
            .IsRequired()
            .HasDefaultValue(true)
            .HasComment("Indicates whether the test setup is active");

        builder.Property(ts => ts.CreatedBy)
            .HasComment("Identifier of the user who created the test setup");

        builder.Property(ts => ts.ModifiedBy)
            .HasComment("Identifier of the user who last modified the test setup");

        // Index for active records
        builder.HasIndex(ts => ts.IsActive)
            .HasDatabaseName("IX_TestSetups_IsActive");

        // Composite index for TypeId and IsActive
        builder.HasIndex(ts => new { ts.TypeId, ts.IsActive })
            .HasDatabaseName("IX_TestSetups_TypeId_IsActive");

        // Relationships configuration
        builder.HasOne(ts => ts.TestType)
            .WithMany(t => t.TestSetups)
            .HasForeignKey(ts => ts.TypeId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_TestSetups_TestTypes");

        builder.HasMany(ts => ts.TestEntries)
            .WithOne(te => te.TestSetup)
            .HasForeignKey(te => te.TestId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_TestEntries_TestSetups");

        // Query filter for soft delete
        builder.HasQueryFilter(ts => ts.IsActive);
    }
}
