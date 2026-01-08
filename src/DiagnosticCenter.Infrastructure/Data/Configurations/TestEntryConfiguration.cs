using DiagnosticCenter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiagnosticCenter.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework Core configuration for the TestEntry entity.
/// Defines table mapping, column specifications, and relationships.
/// </summary>
public class TestEntryConfiguration : IEntityTypeConfiguration<TestEntry>
{
    /// <summary>
    /// Configures the TestEntry entity.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<TestEntry> builder)
    {
        // Table configuration
        builder.ToTable("TestEntries");

        // Primary key configuration
        builder.HasKey(te => te.Id);
        builder.Property(te => te.Id)
            .ValueGeneratedOnAdd()
            .HasComment("Unique identifier for the test entry");

        // Patient information configuration
        builder.Property(te => te.PatientName)
            .IsRequired()
            .HasMaxLength(200)
            .HasComment("Name of the patient");

        builder.HasIndex(te => te.PatientName)
            .HasDatabaseName("IX_TestEntries_PatientName");

        builder.Property(te => te.DateOfBirth)
            .IsRequired()
            .HasComment("Patient's date of birth");

        builder.Property(te => te.MobileNo)
            .IsRequired()
            .HasMaxLength(20)
            .HasComment("Patient's mobile number");

        builder.HasIndex(te => te.MobileNo)
            .HasDatabaseName("IX_TestEntries_MobileNo");

        // Bill information configuration
        builder.Property(te => te.BillNo)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("Bill number for the test entry");

        builder.HasIndex(te => te.BillNo)
            .IsUnique()
            .HasDatabaseName("IX_TestEntries_BillNo");

        // Financial information configuration
        builder.Property(te => te.TotalAmount)
            .IsRequired()
            .HasPrecision(18, 2)
            .HasComment("Total amount for the test");

        builder.Property(te => te.PaidAmount)
            .IsRequired()
            .HasPrecision(18, 2)
            .HasDefaultValue(0)
            .HasComment("Amount already paid");

        builder.Property(te => te.DueDate)
            .IsRequired()
            .HasComment("Due date for payment");

        builder.HasIndex(te => te.DueDate)
            .HasDatabaseName("IX_TestEntries_DueDate");

        // TestId configuration (Foreign Key)
        builder.Property(te => te.TestId)
            .IsRequired()
            .HasComment("Foreign key to the test setup");

        builder.HasIndex(te => te.TestId)
            .HasDatabaseName("IX_TestEntries_TestId");

        // Audit fields configuration
        builder.Property(te => te.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()")
            .HasComment("Date and time when the test entry was created");

        builder.Property(te => te.ModifiedDate)
            .HasComment("Date and time when the test entry was last modified");

        builder.Property(te => te.IsActive)
            .IsRequired()
            .HasDefaultValue(true)
            .HasComment("Indicates whether the test entry is active");

        builder.Property(te => te.CreatedBy)
            .HasComment("Identifier of the user who created the test entry");

        builder.Property(te => te.ModifiedBy)
            .HasComment("Identifier of the user who last modified the test entry");

        // Index for active records
        builder.HasIndex(te => te.IsActive)
            .HasDatabaseName("IX_TestEntries_IsActive");

        // Composite index for common queries
        builder.HasIndex(te => new { te.TestId, te.IsActive })
            .HasDatabaseName("IX_TestEntries_TestId_IsActive");

        // Relationships configuration
        builder.HasOne(te => te.TestSetup)
            .WithMany(ts => ts.TestEntries)
            .HasForeignKey(te => te.TestId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_TestEntries_TestSetups");

        builder.HasMany(te => te.Payments)
            .WithOne(p => p.TestEntry)
            .HasForeignKey(p => p.TestEntryId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Payments_TestEntries");

        // Query filter for soft delete
        builder.HasQueryFilter(te => te.IsActive);
    }
}
