using DiagnosticCenter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiagnosticCenter.Infrastructure.Data.Configurations;

/// <summary>
/// Entity configuration for TestType
/// </summary>
public class TestTypeConfiguration : IEntityTypeConfiguration<TestType>
{
    public void Configure(EntityTypeBuilder<TestType> builder)
    {
        builder.ToTable("test_type");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasColumnName("id");

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("name");

        builder.Property(t => t.Description)
            .HasMaxLength(500)
            .HasColumnName("description");

        builder.Property(t => t.CreatedDate)
            .HasColumnName("created_date")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(t => t.ModifiedDate)
            .HasColumnName("modified_date");

        builder.Property(t => t.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        builder.Property(t => t.CreatedBy)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("created_by");

        builder.Property(t => t.ModifiedBy)
            .HasMaxLength(100)
            .HasColumnName("modified_by");

        builder.HasMany(t => t.TestSetups)
            .WithOne(ts => ts.TestType)
            .HasForeignKey(ts => ts.TypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
