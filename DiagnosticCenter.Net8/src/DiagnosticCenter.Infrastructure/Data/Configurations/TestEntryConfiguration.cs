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
        builder.ToTable("TestEntries");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.PatientName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.DateOfBirth)
            .IsRequired();

        builder.Property(e => e.MobileNo)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(e => e.BillNo)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.TotalAmount)
            .IsRequired()
            .HasColumnType("numeric(18,2)");

        builder.Property(e => e.DueDate)
            .IsRequired();

        builder.Property(e => e.PaidAmount)
            .IsRequired()
            .HasColumnType("numeric(18,2)")
            .HasDefaultValue(0);

        builder.Property(e => e.TestId)
            .IsRequired();

        builder.Property(e => e.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        builder.Property(e => e.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(e => e.CreatedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.ModifiedBy)
            .HasMaxLength(100);

        builder.HasOne(e => e.Test)
            .WithMany(t => t.TestEntries)
            .HasForeignKey(e => e.TestId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => e.BillNo)
            .IsUnique();
        builder.HasIndex(e => e.MobileNo);
        builder.HasIndex(e => e.TestId);
    }
}
