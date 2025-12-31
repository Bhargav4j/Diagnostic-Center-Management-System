using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DiagnosticCenter.Domain.Entities;

namespace DiagnosticCenter.Infrastructure.Data.Configurations;

/// <summary>
/// Entity configuration for TestEntry
/// </summary>
public class TestEntryConfiguration : IEntityTypeConfiguration<TestEntry>
{
    public void Configure(EntityTypeBuilder<TestEntry> builder)
    {
        builder.ToTable("patient");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.PatientName)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("Name");

        builder.Property(e => e.DateOfBirth)
            .IsRequired()
            .HasColumnName("DOB");

        builder.Property(e => e.MobileNo)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(e => e.BillNo)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.TestId)
            .IsRequired()
            .HasColumnName("Test_id");

        builder.Property(e => e.TotalAmount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(e => e.PaidAmount)
            .IsRequired()
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0);

        builder.Property(e => e.DueDate)
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

        builder.HasOne(e => e.TestSetup)
            .WithMany(ts => ts.TestEntries)
            .HasForeignKey(e => e.TestId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Payment)
            .WithOne(p => p.TestEntry)
            .HasForeignKey<Payment>(p => p.TestEntryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.BillNo).IsUnique();
        builder.HasIndex(e => e.MobileNo);
    }
}
