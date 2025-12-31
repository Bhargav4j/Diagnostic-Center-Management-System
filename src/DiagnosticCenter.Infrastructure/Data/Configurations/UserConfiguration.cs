using DiagnosticCenter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiagnosticCenter.Infrastructure.Data.Configurations;

/// <summary>
/// Entity configuration for User
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasColumnName("id");

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("email");

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(500)
            .HasColumnName("password");

        builder.Property(u => u.AccountType)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("acctype");

        builder.Property(u => u.FullName)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("full_name");

        builder.Property(u => u.CreatedDate)
            .HasColumnName("created_date")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(u => u.ModifiedDate)
            .HasColumnName("modified_date");

        builder.Property(u => u.LastLoginDate)
            .HasColumnName("last_login_date");

        builder.Property(u => u.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        builder.Property(u => u.CreatedBy)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("created_by");

        builder.Property(u => u.ModifiedBy)
            .HasMaxLength(100)
            .HasColumnName("modified_by");

        builder.HasIndex(u => u.Email)
            .IsUnique();
    }
}
