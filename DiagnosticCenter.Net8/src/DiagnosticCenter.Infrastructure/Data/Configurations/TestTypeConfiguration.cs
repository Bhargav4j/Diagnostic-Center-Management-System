using DiagnosticCenter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiagnosticCenter.Infrastructure.Data.Configurations
{
    public class TestTypeConfiguration : IEntityTypeConfiguration<TestType>
    {
        public void Configure(EntityTypeBuilder<TestType> builder)
        {
            // Primary Key
            builder.HasKey(t => t.Id);

            // Name constraints
            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            // Description constraints
            builder.Property(t => t.Description)
                .HasMaxLength(500);

            // Unique constraint on Name
            builder.HasIndex(t => t.Name)
                .IsUnique();

            // Default values
            builder.Property(t => t.CreatedDate)
                .HasDefaultValueSql("GETUTCDATE()");

            // Relationship with TestSetup
            builder.HasMany(t => t.TestSetups)
                .WithOne(ts => ts.TestType)
                .HasForeignKey(ts => ts.TypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}