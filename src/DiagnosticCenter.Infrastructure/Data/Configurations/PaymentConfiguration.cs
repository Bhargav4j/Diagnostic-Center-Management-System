using DiagnosticCenter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiagnosticCenter.Infrastructure.Data.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("payments");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder.Property(p => p.BillNo)
            .HasColumnName("bill_no")
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.MobileNo)
            .HasColumnName("mobile_no")
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(p => p.TestName)
            .HasColumnName("test_name")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.TestFee)
            .HasColumnName("test_fee")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(p => p.DueDate)
            .HasColumnName("due_date")
            .IsRequired();

        builder.Property(p => p.TotalAmount)
            .HasColumnName("total_amount")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(p => p.PaidAmount)
            .HasColumnName("paid_amount")
            .HasColumnType("decimal(18,2)")
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(p => p.CreatedDate)
            .HasColumnName("created_at")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(p => p.ModifiedDate)
            .HasColumnName("modified_at");

        builder.Property(p => p.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        builder.Property(p => p.CreatedBy)
            .HasColumnName("created_by")
            .HasMaxLength(100)
            .HasDefaultValue("System");

        builder.Property(p => p.ModifiedBy)
            .HasColumnName("modified_by")
            .HasMaxLength(100);

        builder.Ignore(p => p.DueAmount);

        builder.HasIndex(p => p.BillNo).IsUnique();
        builder.HasIndex(p => p.MobileNo);
    }
}
