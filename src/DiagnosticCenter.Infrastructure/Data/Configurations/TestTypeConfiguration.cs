using DiagnosticCenter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiagnosticCenter.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework Core configuration for the TestType entity.
/// Defines table mapping, column specifications, and relationships.
/// </summary>
public class TestTypeConfiguration : IEntityTypeConfiguration<TestType>
{
    /// <summary>
    /// Configures the TestType entity.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<TestType> builder)
    {
        // Table configuration
        builder.ToTable("TestTypes");

        // Primary key configuration
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id)
            .ValueGeneratedOnAdd()
            .HasComment("Unique identifier for the test type");

        // Name configuration
        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(200)
            .HasComment("Name of the test type");

        builder.HasIndex(t => t.Name)
            .HasDatabaseName("IX_TestTypes_Name");

        // Description configuration
        builder.Property(t => t.Description)
            .HasMaxLength(1000)
            .HasComment("Description of the test type");

        // Audit fields configuration
        builder.Property(t => t.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()")
            .HasComment("Date and time when the test type was created");

        builder.Property(t => t.ModifiedDate)
            .HasComment("Date and time when the test type was last modified");

        builder.Property(t => t.IsActive)
            .IsRequired()
            .HasDefaultValue(true)
            .HasComment("Indicates whether the test type is active");

        builder.Property(t => t.CreatedBy)
            .HasComment("Identifier of the user who created the test type");

        builder.Property(t => t.ModifiedBy)
            .HasComment("Identifier of the user who last modified the test type");

        // Index for active records
        builder.HasIndex(t => t.IsActive)
            .HasDatabaseName("IX_TestTypes_IsActive");

        // Relationships configuration
        builder.HasMany(t => t.TestSetups)
            .WithOne(ts => ts.TestType)
            .HasForeignKey(ts => ts.TypeId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_TestSetups_TestTypes");

        // Query filter for soft delete
        builder.HasQueryFilter(t => t.IsActive);
    }
}
