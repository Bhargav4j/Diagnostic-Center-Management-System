using DiagnosticCenter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiagnosticCenter.Infrastructure.Data.Configurations;

public class TestEntryConfiguration : IEntityTypeConfiguration<TestEntry>
{
    public void Configure(EntityTypeBuilder<TestEntry> builder)
    {
        builder.ToTable("test_entry");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.DOB)
            .IsRequired();

        builder.Property(t => t.MobileNo)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(t => t.BillNo)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(t => t.TotalAmount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(t => t.PaidAmount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(t => t.DueDate)
            .IsRequired();

        builder.Property(t => t.TestId)
            .IsRequired();

        builder.Property(t => t.CreatedDate)
            .IsRequired()
            .HasColumnName("created_at");

        builder.Property(t => t.ModifiedDate)
            .HasColumnName("modified_at");

        builder.Property(t => t.CreatedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(t => t.ModifiedBy)
            .HasMaxLength(100);

        builder.Property(t => t.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasOne(t => t.Test)
            .WithMany(ts => ts.TestEntries)
            .HasForeignKey(t => t.TestId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(t => t.Payments)
            .WithOne(p => p.TestEntry)
            .HasForeignKey(p => p.TestEntryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(t => t.BillNo);
        builder.HasIndex(t => t.MobileNo);
    }
}
