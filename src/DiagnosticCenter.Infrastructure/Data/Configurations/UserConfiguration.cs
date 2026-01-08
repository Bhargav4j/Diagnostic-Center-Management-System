using DiagnosticCenter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiagnosticCenter.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework Core configuration for the User entity.
/// Defines table mapping, column specifications, and relationships.
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    /// <summary>
    /// Configures the User entity.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Table configuration
        builder.ToTable("Users");

        // Primary key configuration
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
            .ValueGeneratedOnAdd()
            .HasComment("Unique identifier for the user");

        // Email configuration
        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(256)
            .HasComment("User's email address");

        builder.HasIndex(u => u.Email)
            .IsUnique()
            .HasDatabaseName("IX_Users_Email");

        // PasswordHash configuration
        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(500)
            .HasComment("Hashed password for the user account");

        // AccountType configuration
        builder.Property(u => u.AccountType)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("Account type for the user (e.g., Admin, Staff)");

        builder.HasIndex(u => u.AccountType)
            .HasDatabaseName("IX_Users_AccountType");

        // Audit fields configuration
        builder.Property(u => u.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()")
            .HasComment("Date and time when the user account was created");

        builder.Property(u => u.ModifiedDate)
            .HasComment("Date and time when the user account was last modified");

        builder.Property(u => u.IsActive)
            .IsRequired()
            .HasDefaultValue(true)
            .HasComment("Indicates whether the user account is active");

        builder.Property(u => u.CreatedBy)
            .HasComment("Identifier of the user who created this user account");

        builder.Property(u => u.ModifiedBy)
            .HasComment("Identifier of the user who last modified this user account");

        // Index for active records
        builder.HasIndex(u => u.IsActive)
            .HasDatabaseName("IX_Users_IsActive");

        // Composite index for common queries
        builder.HasIndex(u => new { u.Email, u.IsActive })
            .HasDatabaseName("IX_Users_Email_IsActive");

        builder.HasIndex(u => new { u.AccountType, u.IsActive })
            .HasDatabaseName("IX_Users_AccountType_IsActive");

        // Query filter for soft delete
        builder.HasQueryFilter(u => u.IsActive);
    }
}
