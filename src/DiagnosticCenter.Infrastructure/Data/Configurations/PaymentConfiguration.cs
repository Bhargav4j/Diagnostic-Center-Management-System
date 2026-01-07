using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DiagnosticCenter.Domain.Entities;

namespace DiagnosticCenter.Infrastructure.Data.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("payment");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("Id");

        builder.Property(e => e.TestEntryId)
            .HasColumnName("test_entry_id")
            .IsRequired();

        builder.Property(e => e.Amount)
            .HasColumnName("amount")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(e => e.PaymentDate)
            .HasColumnName("payment_date")
            .IsRequired();

        builder.Property(e => e.PaymentMode)
            .HasColumnName("payment_mode")
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Notes)
            .HasColumnName("notes")
            .HasMaxLength(500);

        builder.Property(e => e.CreatedDate)
            .HasColumnName("created_date")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(e => e.TestEntry)
            .WithMany(e => e.Payments)
            .HasForeignKey(e => e.TestEntryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
