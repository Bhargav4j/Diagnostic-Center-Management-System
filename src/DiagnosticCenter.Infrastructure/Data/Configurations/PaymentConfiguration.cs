using DiagnosticCenter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiagnosticCenter.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework Core configuration for the Payment entity.
/// Defines table mapping, column specifications, and relationships.
/// </summary>
public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    /// <summary>
    /// Configures the Payment entity.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        // Table configuration
        builder.ToTable("Payments");

        // Primary key configuration
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd()
            .HasComment("Unique identifier for the payment");

        // BillNo configuration
        builder.Property(p => p.BillNo)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("Bill number associated with this payment");

        builder.HasIndex(p => p.BillNo)
            .HasDatabaseName("IX_Payments_BillNo");

        // Amount configuration
        builder.Property(p => p.Amount)
            .IsRequired()
            .HasPrecision(18, 2)
            .HasComment("Payment amount");

        // PaymentDate configuration
        builder.Property(p => p.PaymentDate)
            .IsRequired()
            .HasComment("Date and time when the payment was made");

        builder.HasIndex(p => p.PaymentDate)
            .HasDatabaseName("IX_Payments_PaymentDate");

        // TestEntryId configuration (Foreign Key)
        builder.Property(p => p.TestEntryId)
            .IsRequired()
            .HasComment("Foreign key to the test entry");

        builder.HasIndex(p => p.TestEntryId)
            .HasDatabaseName("IX_Payments_TestEntryId");

        // Audit fields configuration
        builder.Property(p => p.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()")
            .HasComment("Date and time when the payment record was created");

        builder.Property(p => p.ModifiedDate)
            .HasComment("Date and time when the payment record was last modified");

        builder.Property(p => p.IsActive)
            .IsRequired()
            .HasDefaultValue(true)
            .HasComment("Indicates whether the payment record is active");

        builder.Property(p => p.CreatedBy)
            .HasComment("Identifier of the user who created the payment record");

        builder.Property(p => p.ModifiedBy)
            .HasComment("Identifier of the user who last modified the payment record");

        // Index for active records
        builder.HasIndex(p => p.IsActive)
            .HasDatabaseName("IX_Payments_IsActive");

        // Composite index for common queries
        builder.HasIndex(p => new { p.TestEntryId, p.IsActive })
            .HasDatabaseName("IX_Payments_TestEntryId_IsActive");

        builder.HasIndex(p => new { p.BillNo, p.PaymentDate })
            .HasDatabaseName("IX_Payments_BillNo_PaymentDate");

        // Relationships configuration
        builder.HasOne(p => p.TestEntry)
            .WithMany(te => te.Payments)
            .HasForeignKey(p => p.TestEntryId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Payments_TestEntries");

        // Query filter for soft delete
        builder.HasQueryFilter(p => p.IsActive);
    }
}
