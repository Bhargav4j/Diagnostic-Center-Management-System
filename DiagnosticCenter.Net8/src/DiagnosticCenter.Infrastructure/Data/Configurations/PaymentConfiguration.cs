using DiagnosticCenter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiagnosticCenter.Infrastructure.Data.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            // Primary Key
            builder.HasKey(p => p.Id);

            // TestEntry relationship (required)
            builder.HasOne(p => p.TestEntry)
                .WithMany(te => te.Payments)
                .HasForeignKey(p => p.TestEntryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            // BillNo constraints
            builder.Property(p => p.BillNo)
                .IsRequired()
                .HasMaxLength(50);

            // MobileNo constraints
            builder.Property(p => p.MobileNo)
                .IsRequired()
                .HasMaxLength(20);

            // Amount constraints
            builder.Property(p => p.Amount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            // Default values
            builder.Property(p => p.CreatedDate)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(p => p.PaymentDate)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}