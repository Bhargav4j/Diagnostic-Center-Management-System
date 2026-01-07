using DiagnosticCenter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiagnosticCenter.Infrastructure.Data.Configurations;

public class TestEntryConfiguration : IEntityTypeConfiguration<TestEntry>
{
    public void Configure(EntityTypeBuilder<TestEntry> builder)
    {
        builder.ToTable("TestEntries");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.PatientName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.MobileNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(e => e.BillNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.TotalAmount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(e => e.PaidAmount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(e => e.DateOfBirth)
            .IsRequired();

        builder.Property(e => e.DueDate)
            .IsRequired();

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

        builder.Ignore(e => e.DueAmount);

        builder.HasMany(e => e.TestEntryItems)
            .WithOne(e => e.TestEntry)
            .HasForeignKey(e => e.TestEntryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Payments)
            .WithOne(e => e.TestEntry)
            .HasForeignKey(e => e.TestEntryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => e.BillNumber).IsUnique();
        builder.HasIndex(e => e.MobileNumber);
        builder.HasIndex(e => e.PatientName);
    }
}
