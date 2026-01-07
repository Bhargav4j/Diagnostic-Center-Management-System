using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DiagnosticCenter.Domain.Entities;

namespace DiagnosticCenter.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("Id");

        builder.Property(e => e.Email)
            .HasColumnName("email")
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.PasswordHash)
            .HasColumnName("password")
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.AccountType)
            .HasColumnName("acctype")
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.IsActive)
            .HasDefaultValue(true);

        builder.Property(e => e.CreatedDate)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.HasIndex(e => e.Email)
            .IsUnique();
    }
}
