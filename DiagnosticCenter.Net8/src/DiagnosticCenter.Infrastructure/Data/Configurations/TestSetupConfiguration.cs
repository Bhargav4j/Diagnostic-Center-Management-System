using DiagnosticCenter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiagnosticCenter.Infrastructure.Data.Configurations
{
    public class TestSetupConfiguration : IEntityTypeConfiguration<TestSetup>
    {
        public void Configure(EntityTypeBuilder<TestSetup> builder)
        {
            // Primary Key
            builder.HasKey(ts => ts.Id);

            // TestType relationship (required)
            builder.HasOne(ts => ts.TestType)
                .WithMany(tt => tt.TestSetups)
                .HasForeignKey(ts => ts.TypeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            // Fee constraints
            builder.Property(ts => ts.Fee)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            // Default values
            builder.Property(ts => ts.CreatedDate)
                .HasDefaultValueSql("GETUTCDATE()");

            // Relationship with TestEntry
            builder.HasMany(ts => ts.TestEntries)
                .WithOne(te => te.TestSetup)
                .HasForeignKey(te => te.TestId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}