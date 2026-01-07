using DiagnosticCenter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiagnosticCenter.Infrastructure.Data.Configurations;

public class TestEntryItemConfiguration : IEntityTypeConfiguration<TestEntryItem>
{
    public void Configure(EntityTypeBuilder<TestEntryItem> builder)
    {
        builder.ToTable("TestEntryItems");

        builder.HasKey(e => e.Id);

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

        builder.HasIndex(e => e.TestEntryId);
        builder.HasIndex(e => e.TestSetupId);
    }
}
