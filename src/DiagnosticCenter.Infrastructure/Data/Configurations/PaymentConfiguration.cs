using DiagnosticCenter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiagnosticCenter.Infrastructure.Data.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("payments", "public");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.PatientTestId)
            .IsRequired();

        builder.Property(p => p.BillNo)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnType("varchar(100)");

        builder.Property(p => p.AmountPaid)
            .IsRequired()
            .HasColumnType("numeric(18,2)");

        builder.Property(p => p.PaymentDate)
            .IsRequired()
            .HasColumnType("timestamp without time zone");

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

        builder.HasIndex(p => p.BillNo);
    }
}
