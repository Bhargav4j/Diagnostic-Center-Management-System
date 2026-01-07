using DiagnosticCenter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiagnosticCenter.Infrastructure.Data.Configurations;

public class TestTypeConfiguration : IEntityTypeConfiguration<TestType>
{
    public void Configure(EntityTypeBuilder<TestType> builder)
    {
        builder.ToTable("TestTypes");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Description)
            .HasMaxLength(1000);

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

        builder.HasMany(e => e.TestSetups)
            .WithOne(e => e.TestType)
            .HasForeignKey(e => e.TestTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => e.Name);
    }
}
