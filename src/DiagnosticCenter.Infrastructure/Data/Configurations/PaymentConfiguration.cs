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

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("id");

        builder.Property(p => p.BillNo)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("bill_no");

        builder.Property(p => p.TestEntryId)
            .HasColumnName("test_entry_id");

        builder.Property(p => p.Amount)
            .HasColumnType("decimal(18,2)")
            .HasColumnName("amount");

        builder.Property(p => p.PaymentDate)
            .HasColumnName("payment_date");

        builder.Property(p => p.PaymentMethod)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("payment_method");

        builder.Property(p => p.TransactionReference)
            .HasMaxLength(100)
            .HasColumnName("transaction_reference");

        builder.Property(p => p.CreatedDate)
            .HasColumnName("created_date")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(p => p.ModifiedDate)
            .HasColumnName("modified_date");

        builder.Property(p => p.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        builder.Property(p => p.CreatedBy)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("created_by");

        builder.Property(p => p.ModifiedBy)
            .HasMaxLength(100)
            .HasColumnName("modified_by");

        builder.HasOne(p => p.TestEntry)
            .WithMany(te => te.Payments)
            .HasForeignKey(p => p.TestEntryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
