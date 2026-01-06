using DiagnosticCenter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiagnosticCenter.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // AccountType constraints
            builder.Property(u => u.AccountType)
                .IsRequired()
                .HasMaxLength(50);

            // Default values
            builder.Property(u => u.CreatedDate)
                .HasDefaultValueSql("GETUTCDATE()");

            // Index on AccountType
            builder.HasIndex(u => u.AccountType);
        }
    }
}