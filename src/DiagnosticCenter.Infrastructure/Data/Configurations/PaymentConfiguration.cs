using DiagnosticCenter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiagnosticCenter.Infrastructure.Data.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("payment");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.BillNo)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.MobileNo)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(p => p.TotalAmount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.PaidAmount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.PaymentDate)
            .IsRequired();

        builder.Property(p => p.TestEntryId)
            .IsRequired();

        builder.Property(p => p.CreatedDate)
            .IsRequired()
            .HasColumnName("created_at");

        builder.Property(p => p.ModifiedDate)
            .HasColumnName("modified_at");

        builder.Property(p => p.CreatedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.ModifiedBy)
            .HasMaxLength(100);

        builder.Property(p => p.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasOne(p => p.TestEntry)
            .WithMany(te => te.Payments)
            .HasForeignKey(p => p.TestEntryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(p => p.BillNo);
    }
}
