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

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("Id");

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("Name");

        builder.Property(e => e.Description)
            .HasMaxLength(500)
            .HasColumnName("Description");

        builder.Property(e => e.CreatedDate)
            .IsRequired()
            .HasColumnName("created_at")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(e => e.ModifiedDate)
            .HasColumnName("modified_at");

        builder.Property(e => e.IsActive)
            .IsRequired()
            .HasColumnName("IsActive")
            .HasDefaultValue(true);

        builder.Property(e => e.CreatedBy)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("CreatedBy");

        builder.Property(e => e.ModifiedBy)
            .HasMaxLength(100)
            .HasColumnName("ModifiedBy");

        builder.HasMany(e => e.TestSetups)
            .WithOne(e => e.TestType)
            .HasForeignKey(e => e.TestTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => e.Name);
    }
}
