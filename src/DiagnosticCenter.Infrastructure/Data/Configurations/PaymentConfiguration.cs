using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DiagnosticCenter.Domain.Entities;

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

        builder.Property(e => e.TestEntryId)
            .IsRequired();

        builder.Property(e => e.BillNo)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.TotalAmount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(e => e.PaidAmount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(e => e.PaymentDate)
            .IsRequired();

        builder.Property(e => e.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");

        builder.Property(e => e.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(e => e.CreatedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.ModifiedBy)
            .HasMaxLength(100);

        builder.HasOne(e => e.TestEntry)
            .WithOne(te => te.Payment)
            .HasForeignKey<Payment>(e => e.TestEntryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.BillNo);
    }
}
