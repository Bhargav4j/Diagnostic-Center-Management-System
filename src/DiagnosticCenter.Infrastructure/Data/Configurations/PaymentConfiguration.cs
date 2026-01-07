using DiagnosticCenter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiagnosticCenter.Infrastructure.Data.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.BillNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(e => e.PaymentDate)
            .IsRequired();

        builder.Property(e => e.PaymentMethod)
            .HasMaxLength(50);

        builder.Property(e => e.TransactionReference)
            .HasMaxLength(100);

        builder.Property(e => e.Notes)
            .HasMaxLength(500);

        builder.Property(e => e.CreatedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.ModifiedBy)
            .HasMaxLength(100);

        builder.Property(e => e.CreatedDate)
            .IsRequired();

        builder.Property(e => e.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasIndex(e => e.BillNumber);
        builder.HasIndex(e => e.TestEntryId);
        builder.HasIndex(e => e.PaymentDate);
    }
}
