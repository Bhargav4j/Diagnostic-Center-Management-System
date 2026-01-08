using DiagnosticCenter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiagnosticCenter.Infrastructure.Data.Configurations;

public class TestSetupConfiguration : IEntityTypeConfiguration<TestSetup>
{
    public void Configure(EntityTypeBuilder<TestSetup> builder)
    {
        builder.ToTable("test_name");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder.Property(t => t.Name)
            .HasColumnName("Name")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.Fee)
            .HasColumnName("Fee")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(t => t.TypeId)
            .HasColumnName("type_id")
            .IsRequired();

        builder.Property(t => t.CreatedDate)
            .HasColumnName("created_at")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(t => t.ModifiedDate)
            .HasColumnName("modified_at");

        builder.Property(t => t.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        builder.Property(t => t.CreatedBy)
            .HasColumnName("created_by")
            .HasMaxLength(100)
            .HasDefaultValue("System");

        builder.Property(t => t.ModifiedBy)
            .HasColumnName("modified_by")
            .HasMaxLength(100);

        builder.HasOne(t => t.TestType)
            .WithMany(tt => tt.TestSetups)
            .HasForeignKey(t => t.TypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(t => t.Name);
        builder.HasIndex(t => t.TypeId);
    }
}
