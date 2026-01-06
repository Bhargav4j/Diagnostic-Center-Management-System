using DiagnosticCenter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiagnosticCenter.Infrastructure.Data.Configurations
{
    public class TestEntryConfiguration : IEntityTypeConfiguration<TestEntry>
    {
        public void Configure(EntityTypeBuilder<TestEntry> builder)
        {
            // Primary Key
            builder.HasKey(te => te.Id);

            // TestSetup relationship (required)
            builder.HasOne(te => te.TestSetup)
                .WithMany(ts => ts.TestEntries)
                .HasForeignKey(te => te.TestId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            // Name constraints
            builder.Property(te => te.Name)
                .IsRequired()
                .HasMaxLength(200);

            // BillNo constraints
            builder.Property(te => te.BillNo)
                .IsRequired()
                .HasMaxLength(50);

            // MobileNo constraints
            builder.Property(te => te.MobileNo)
                .IsRequired()
                .HasMaxLength(20);

            // Amount constraints
            builder.Property(te => te.TotalAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(te => te.PaidAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            // Default values
            builder.Property(te => te.CreatedDate)
                .HasDefaultValueSql("GETUTCDATE()");

            // Relationship with Payment
            builder.HasMany(te => te.Payments)
                .WithOne(p => p.TestEntry)
                .HasForeignKey(p => p.TestEntryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}