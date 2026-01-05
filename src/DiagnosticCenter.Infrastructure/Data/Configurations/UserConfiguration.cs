using DiagnosticCenter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiagnosticCenter.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for User entity
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasColumnName("Id");

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
            .HasMaxLength(200)
            .HasColumnName("FullName");

        builder.Property(u => u.CreatedDate)
            .IsRequired()
            .HasColumnName("created_at")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(u => u.ModifiedDate)
            .HasColumnName("modified_at");

        builder.Property(u => u.IsActive)
            .IsRequired()
            .HasDefaultValue(true)
            .HasColumnName("IsActive");

        builder.Property(u => u.CreatedBy)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("CreatedBy")
            .HasDefaultValue("System");

        builder.Property(u => u.ModifiedBy)
            .HasMaxLength(100)
            .HasColumnName("ModifiedBy");

        builder.HasIndex(u => u.Email).IsUnique();
    }
}
