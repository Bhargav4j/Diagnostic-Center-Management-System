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
            .HasColumnName("Id");

        builder.Property(p => p.BillNumber)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("BillNo");

        builder.Property(p => p.TestEntryId)
            .IsRequired()
            .HasColumnName("TestEntryId");

        builder.Property(p => p.Amount)
            .IsRequired()
            .HasPrecision(18, 2)
            .HasColumnName("Amount");

        builder.Property(p => p.PaymentDate)
            .IsRequired()
            .HasColumnName("PaymentDate")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(p => p.CreatedDate)
            .IsRequired()
            .HasColumnName("created_at")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(p => p.ModifiedDate)
            .HasColumnName("modified_at");

        builder.Property(p => p.IsActive)
            .IsRequired()
            .HasDefaultValue(true)
            .HasColumnName("IsActive");

        builder.Property(p => p.CreatedBy)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("CreatedBy");

        builder.Property(p => p.ModifiedBy)
            .HasMaxLength(100)
            .HasColumnName("ModifiedBy");

        builder.HasOne(p => p.TestEntry)
            .WithMany(te => te.Payments)
            .HasForeignKey(p => p.TestEntryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(p => p.BillNumber);
    }
}
