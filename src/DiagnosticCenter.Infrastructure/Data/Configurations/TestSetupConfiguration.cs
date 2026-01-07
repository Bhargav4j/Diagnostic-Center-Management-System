using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DiagnosticCenter.Domain.Entities;

namespace DiagnosticCenter.Infrastructure.Data.Configurations;

public class TestSetupConfiguration : IEntityTypeConfiguration<TestSetup>
{
    public void Configure(EntityTypeBuilder<TestSetup> builder)
    {
        builder.ToTable("test_name");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("Id");

        builder.Property(e => e.Name)
            .HasColumnName("Name")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Fee)
            .HasColumnName("Fee")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(e => e.TypeId)
            .HasColumnName("type_id")
            .IsRequired();

        builder.Property(e => e.IsActive)
            .HasDefaultValue(true);

        builder.Property(e => e.CreatedDate)
            .HasColumnName("created_at")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(e => e.TestType)
            .WithMany(e => e.TestSetups)
            .HasForeignKey(e => e.TypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.TestEntries)
            .WithOne(e => e.TestSetup)
            .HasForeignKey(e => e.TestSetupId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
