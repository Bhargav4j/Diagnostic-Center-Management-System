using DiagnosticCenter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiagnosticCenter.Infrastructure.Data.Configurations;

/// <summary>
/// Entity configuration for TestEntry
/// </summary>
public class TestEntryConfiguration : IEntityTypeConfiguration<TestEntry>
{
    public void Configure(EntityTypeBuilder<TestEntry> builder)
    {
        builder.ToTable("test_entry");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasColumnName("id");

        builder.Property(t => t.PatientName)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("patient_name");

        builder.Property(t => t.DateOfBirth)
            .HasColumnName("date_of_birth");

        builder.Property(t => t.MobileNo)
            .IsRequired()
            .HasMaxLength(20)
            .HasColumnName("mobile_no");

        builder.Property(t => t.BillNo)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("bill_no");

        builder.Property(t => t.TotalAmount)
            .HasColumnType("decimal(18,2)")
            .HasColumnName("total_amount");

        builder.Property(t => t.DueDate)
            .HasColumnName("due_date");

        builder.Property(t => t.PaidAmount)
            .HasColumnType("decimal(18,2)")
            .HasColumnName("paid_amount");

        builder.Property(t => t.TestId)
            .HasColumnName("test_id");

        builder.Property(t => t.CreatedDate)
            .HasColumnName("created_date")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(t => t.ModifiedDate)
            .HasColumnName("modified_date");

        builder.Property(t => t.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        builder.Property(t => t.CreatedBy)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("created_by");

        builder.Property(t => t.ModifiedBy)
            .HasMaxLength(100)
            .HasColumnName("modified_by");

        builder.HasOne(t => t.Test)
            .WithMany(ts => ts.TestEntries)
            .HasForeignKey(t => t.TestId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(t => t.Payments)
            .WithOne(p => p.TestEntry)
            .HasForeignKey(p => p.TestEntryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(t => t.BillNo)
            .IsUnique();
    }
}
