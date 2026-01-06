using DiagnosticCenter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiagnosticCenter.Infrastructure.Data.Configurations;

public class TestTypeConfiguration : IEntityTypeConfiguration<TestType>
{
    public void Configure(EntityTypeBuilder<TestType> builder)
    {
        builder.ToTable("test_type");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(200);

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

        builder.HasMany(t => t.TestSetups)
            .WithOne(ts => ts.TestType)
            .HasForeignKey(ts => ts.TypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
