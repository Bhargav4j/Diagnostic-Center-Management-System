using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DiagnosticCenter.Domain.Entities;

namespace DiagnosticCenter.Infrastructure.Data.Configurations;

public class TestEntryConfiguration : IEntityTypeConfiguration<TestEntry>
{
    public void Configure(EntityTypeBuilder<TestEntry> builder)
    {
        builder.ToTable("test_entry");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("Id");

        builder.Property(e => e.BillNo)
            .HasColumnName("bill_no")
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.PatientName)
            .HasColumnName("patient_name")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.PatientAge)
            .HasColumnName("patient_age")
            .IsRequired();

        builder.Property(e => e.PatientGender)
            .HasColumnName("patient_gender")
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(e => e.ContactNumber)
            .HasColumnName("contact_number")
            .HasMaxLength(20);

        builder.Property(e => e.TestSetupId)
            .HasColumnName("test_setup_id")
            .IsRequired();

        builder.Property(e => e.TestDate)
            .HasColumnName("test_date")
            .IsRequired();

        builder.Property(e => e.TotalFee)
            .HasColumnName("total_fee")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(e => e.IsPaid)
            .HasColumnName("is_paid")
            .HasDefaultValue(false);

        builder.Property(e => e.CreatedDate)
            .HasColumnName("created_date")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(e => e.TestSetup)
            .WithMany(e => e.TestEntries)
            .HasForeignKey(e => e.TestSetupId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Payments)
            .WithOne(e => e.TestEntry)
            .HasForeignKey(e => e.TestEntryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.BillNo)
            .IsUnique();
    }
}
