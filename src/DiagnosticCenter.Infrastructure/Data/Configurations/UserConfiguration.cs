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

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("Id");

        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("email");

        builder.Property(e => e.PasswordHash)
            .IsRequired()
            .HasMaxLength(500)
            .HasColumnName("password");

        builder.Property(e => e.AccountType)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("acctype");

        builder.Property(e => e.FullName)
            .HasMaxLength(200)
            .HasColumnName("FullName");

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

        builder.HasIndex(e => e.Email)
            .IsUnique();
    }
}
