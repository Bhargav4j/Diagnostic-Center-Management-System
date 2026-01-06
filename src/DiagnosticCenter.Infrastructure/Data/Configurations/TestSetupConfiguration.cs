using DiagnosticCenter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiagnosticCenter.Infrastructure.Data.Configurations;

public class TestSetupConfiguration : IEntityTypeConfiguration<TestSetup>
{
    public void Configure(EntityTypeBuilder<TestSetup> builder)
    {
        builder.ToTable("test_setup");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.Fee)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(t => t.TypeId)
            .IsRequired();

        builder.Property(t => t.Description)
            .HasMaxLength(500);

        builder.Property(t => t.CreatedDate)
            .IsRequired()
            .HasColumnName("created_at");

        builder.Property(t => t.ModifiedDate)
            .HasColumnName("modified_at");

        builder.Property(t => t.CreatedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(t => t.ModifiedBy)
            .HasMaxLength(100);

        builder.Property(t => t.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasOne(t => t.TestType)
            .WithMany(tt => tt.TestSetups)
            .HasForeignKey(t => t.TypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(t => t.TestEntries)
            .WithOne(te => te.Test)
            .HasForeignKey(te => te.TestId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
