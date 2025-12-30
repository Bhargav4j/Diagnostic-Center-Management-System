using DiagnosticCenter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiagnosticCenter.Infrastructure.Data.Configurations;

/// <summary>
/// Entity configuration for Payment
/// </summary>
public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("payment");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.BillNo)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Amount)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(e => e.PaymentDate)
            .IsRequired();

        builder.Property(e => e.PaymentMethod)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Notes)
            .HasMaxLength(500);

        builder.Property(e => e.TestEntryId)
            .IsRequired();

        builder.Property(e => e.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(e => e.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(e => e.CreatedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.ModifiedBy)
            .HasMaxLength(100);

        // Relationships
        builder.HasOne(e => e.TestEntry)
            .WithMany(t => t.Payments)
            .HasForeignKey(e => e.TestEntryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(e => e.BillNo);
        builder.HasIndex(e => e.TestEntryId);
        builder.HasIndex(e => e.PaymentDate);
    }
}
