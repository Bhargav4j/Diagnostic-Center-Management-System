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
            .HasColumnName("Id");

        builder.Property(t => t.PatientName)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("Name");

        builder.Property(t => t.DateOfBirth)
            .IsRequired()
            .HasColumnName("DOB");

        builder.Property(t => t.MobileNumber)
            .IsRequired()
            .HasMaxLength(20)
            .HasColumnName("MobileNo");

        builder.Property(t => t.BillNumber)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("BillNo");

        builder.Property(t => t.TotalAmount)
            .IsRequired()
            .HasPrecision(18, 2)
            .HasColumnName("TotalAmount");

        builder.Property(t => t.DueDate)
            .IsRequired()
            .HasColumnName("DueDate");

        builder.Property(t => t.PaidAmount)
            .IsRequired()
            .HasPrecision(18, 2)
            .HasColumnName("PaidAmount")
            .HasDefaultValue(0);

        builder.Property(t => t.TestSetupId)
            .IsRequired()
            .HasColumnName("TestId");

        builder.Property(t => t.CreatedDate)
            .IsRequired()
            .HasColumnName("created_at")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(t => t.ModifiedDate)
            .HasColumnName("modified_at");

        builder.Property(t => t.IsActive)
            .IsRequired()
            .HasDefaultValue(true)
            .HasColumnName("IsActive");

        builder.Property(t => t.CreatedBy)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("CreatedBy");

        builder.Property(t => t.ModifiedBy)
            .HasMaxLength(100)
            .HasColumnName("ModifiedBy");

        builder.HasOne(t => t.TestSetup)
            .WithMany(ts => ts.TestEntries)
            .HasForeignKey(t => t.TestSetupId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(t => t.Payments)
            .WithOne(p => p.TestEntry)
            .HasForeignKey(p => p.TestEntryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(t => t.BillNumber).IsUnique();
        builder.HasIndex(t => t.MobileNumber);
    }
}
