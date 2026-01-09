using DiagnosticCenter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiagnosticCenter.Infrastructure.Data.Configurations;

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.ToTable("patients", "public");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnType("varchar(200)");

        builder.Property(p => p.DateOfBirth)
            .IsRequired()
            .HasColumnType("timestamp without time zone");

        builder.Property(p => p.MobileNo)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnType("varchar(50)");

        builder.Property(p => p.CreatedBy)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnType("varchar(100)");

        builder.Property(p => p.ModifiedBy)
            .HasMaxLength(100)
            .HasColumnType("varchar(100)");

        builder.Property(p => p.CreatedDate)
            .IsRequired()
            .HasColumnType("timestamp without time zone");

        builder.Property(p => p.ModifiedDate)
            .HasColumnType("timestamp without time zone");

        builder.Property(p => p.IsActive)
            .IsRequired()
            .HasDefaultValue(true)
            .HasColumnType("boolean");

        builder.HasIndex(p => p.MobileNo);

        builder.HasMany(p => p.PatientTests)
            .WithOne(pt => pt.Patient)
            .HasForeignKey(pt => pt.PatientId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
