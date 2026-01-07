using DiagnosticCenter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiagnosticCenter.Infrastructure.Data.Configurations;

public class TestSetupConfiguration : IEntityTypeConfiguration<TestSetup>
{
    public void Configure(EntityTypeBuilder<TestSetup> builder)
    {
        builder.ToTable("TestSetups");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Description)
            .HasMaxLength(1000);

        builder.Property(e => e.Fee)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

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

        builder.HasMany(e => e.TestEntryItems)
            .WithOne(e => e.TestSetup)
            .HasForeignKey(e => e.TestSetupId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => e.Name);
        builder.HasIndex(e => e.TestTypeId);
    }
}
