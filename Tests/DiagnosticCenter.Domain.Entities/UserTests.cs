using Xunit;
using DiagnosticCenter.Domain.Entities;
using System;

namespace DiagnosticCenter.Domain.Entities.Tests;

public class UserTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        Assert.NotNull(user);
        Assert.Equal(string.Empty, user.Email);
        Assert.Equal(string.Empty, user.PasswordHash);
        Assert.Equal(string.Empty, user.AccountType);
        Assert.Equal("System", user.CreatedBy);
    }

    [Fact]
    public void Id_SetAndGet_ShouldWork()
    {
        // Arrange
        var user = new User();
        var expectedId = 100;

        // Act
        user.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, user.Id);
    }

    [Fact]
    public void Email_SetAndGet_ShouldWork()
    {
        // Arrange
        var user = new User();
        var expectedEmail = "admin@example.com";

        // Act
        user.Email = expectedEmail;

        // Assert
        Assert.Equal(expectedEmail, user.Email);
    }

    [Fact]
    public void PasswordHash_SetAndGet_ShouldWork()
    {
        // Arrange
        var user = new User();
        var expectedHash = "hashedpassword123";

        // Act
        user.PasswordHash = expectedHash;

        // Assert
        Assert.Equal(expectedHash, user.PasswordHash);
    }

    [Fact]
    public void AccountType_SetAndGet_ShouldWork()
    {
        // Arrange
        var user = new User();
        var expectedAccountType = "Admin";

        // Act
        user.AccountType = expectedAccountType;

        // Assert
        Assert.Equal(expectedAccountType, user.AccountType);
    }

    [Fact]
    public void AccountType_SetReceptionist_ShouldWork()
    {
        // Arrange
        var user = new User();

        // Act
        user.AccountType = "Receptionist";

        // Assert
        Assert.Equal("Receptionist", user.AccountType);
    }

    [Fact]
    public void AccountType_SetAccountant_ShouldWork()
    {
        // Arrange
        var user = new User();

        // Act
        user.AccountType = "Accountant";

        // Assert
        Assert.Equal("Accountant", user.AccountType);
    }

    [Fact]
    public void FullName_SetAndGet_ShouldWork()
    {
        // Arrange
        var user = new User();
        var expectedFullName = "John Administrator";

        // Act
        user.FullName = expectedFullName;

        // Assert
        Assert.Equal(expectedFullName, user.FullName);
    }

    [Fact]
    public void FullName_SetNull_ShouldWork()
    {
        // Arrange
        var user = new User();

        // Act
        user.FullName = null;

        // Assert
        Assert.Null(user.FullName);
    }

    [Fact]
    public void CreatedDate_SetAndGet_ShouldWork()
    {
        // Arrange
        var user = new User();
        var expectedDate = DateTime.UtcNow;

        // Act
        user.CreatedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, user.CreatedDate);
    }

    [Fact]
    public void ModifiedDate_SetAndGet_ShouldWork()
    {
        // Arrange
        var user = new User();
        var expectedDate = DateTime.UtcNow;

        // Act
        user.ModifiedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, user.ModifiedDate);
    }

    [Fact]
    public void IsActive_SetAndGet_ShouldWork()
    {
        // Arrange
        var user = new User();

        // Act
        user.IsActive = true;

        // Assert
        Assert.True(user.IsActive);
    }

    [Fact]
    public void IsActive_SetFalse_ShouldWork()
    {
        // Arrange
        var user = new User();

        // Act
        user.IsActive = false;

        // Assert
        Assert.False(user.IsActive);
    }

    [Fact]
    public void CreatedBy_DefaultValue_ShouldBeSystem()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        Assert.Equal("System", user.CreatedBy);
    }

    [Fact]
    public void CreatedBy_SetAndGet_ShouldWork()
    {
        // Arrange
        var user = new User();
        var expectedCreatedBy = "Administrator";

        // Act
        user.CreatedBy = expectedCreatedBy;

        // Assert
        Assert.Equal(expectedCreatedBy, user.CreatedBy);
    }

    [Fact]
    public void ModifiedBy_SetAndGet_ShouldWork()
    {
        // Arrange
        var user = new User();
        var expectedModifiedBy = "Admin";

        // Act
        user.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedModifiedBy, user.ModifiedBy);
    }

    [Fact]
    public void AllProperties_SetAndGet_ShouldWork()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Email = "test@example.com",
            PasswordHash = "hash123",
            AccountType = "Admin",
            FullName = "Test User",
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            IsActive = true,
            CreatedBy = "System",
            ModifiedBy = "Admin"
        };

        // Assert
        Assert.Equal(1, user.Id);
        Assert.Equal("test@example.com", user.Email);
        Assert.Equal("hash123", user.PasswordHash);
        Assert.Equal("Admin", user.AccountType);
        Assert.Equal("Test User", user.FullName);
        Assert.True(user.IsActive);
        Assert.Equal("System", user.CreatedBy);
        Assert.Equal("Admin", user.ModifiedBy);
    }
}
