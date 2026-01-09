using DiagnosticCenter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiagnosticCenter.Infrastructure.Data.Configurations;

public class PatientTestConfiguration : IEntityTypeConfiguration<PatientTest>
{
    public void Configure(EntityTypeBuilder<PatientTest> builder)
    {
        builder.ToTable("PatientTests");

        builder.HasKey(pt => pt.Id);

        builder.Property(pt => pt.PatientId)
            .IsRequired();

        builder.Property(pt => pt.TestSetupId)
            .IsRequired();

        builder.Property(pt => pt.BillNo)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(pt => pt.TotalAmount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(pt => pt.PaidAmount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(pt => pt.DueDate)
            .IsRequired();

        builder.Property(pt => pt.CreatedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(pt => pt.ModifiedBy)
            .HasMaxLength(100);

        builder.Property(pt => pt.CreatedDate)
            .IsRequired();

        builder.Property(pt => pt.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasIndex(pt => pt.BillNo)
            .IsUnique();

        builder.HasOne(pt => pt.Payment)
            .WithOne(p => p.PatientTest)
            .HasForeignKey<Payment>(p => p.PatientTestId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
