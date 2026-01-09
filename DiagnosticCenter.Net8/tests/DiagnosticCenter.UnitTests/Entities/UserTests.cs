using Xunit;
using DiagnosticCenter.Domain.Entities;
using System;

namespace DiagnosticCenter.UnitTests.Entities;

public class UserTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        Assert.Equal(string.Empty, user.FullName);
        Assert.False(user.IsActive);
        Assert.Null(user.ModifiedDate);
    }

    [Fact]
    public void FullName_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var user = new User();
        var expectedFullName = "John Doe";

        // Act
        user.FullName = expectedFullName;

        // Assert
        Assert.Equal(expectedFullName, user.FullName);
    }

    [Fact]
    public void CreatedDate_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var user = new User();
        var expectedDate = DateTime.Now;

        // Act
        user.CreatedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, user.CreatedDate);
    }

    [Fact]
    public void ModifiedDate_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var user = new User();
        var expectedDate = DateTime.Now;

        // Act
        user.ModifiedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, user.ModifiedDate);
    }

    [Fact]
    public void ModifiedDate_ShouldAcceptNull()
    {
        // Arrange
        var user = new User { ModifiedDate = DateTime.Now };

        // Act
        user.ModifiedDate = null;

        // Assert
        Assert.Null(user.ModifiedDate);
    }

    [Fact]
    public void IsActive_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var user = new User();

        // Act
        user.IsActive = true;

        // Assert
        Assert.True(user.IsActive);
    }

    [Fact]
    public void User_ShouldInheritFromIdentityUser()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        Assert.IsAssignableFrom<Microsoft.AspNetCore.Identity.IdentityUser>(user);
    }

    [Fact]
    public void User_ShouldAllowSettingIdentityUserProperties()
    {
        // Arrange
        var user = new User();

        // Act
        user.UserName = "johndoe";
        user.Email = "john@example.com";
        user.PhoneNumber = "1234567890";

        // Assert
        Assert.Equal("johndoe", user.UserName);
        Assert.Equal("john@example.com", user.Email);
        Assert.Equal("1234567890", user.PhoneNumber);
    }

    [Fact]
    public void User_ShouldAllowCompleteObjectInitialization()
    {
        // Arrange
        var createdDate = DateTime.Now;
        var modifiedDate = DateTime.Now.AddDays(1);

        // Act
        var user = new User
        {
            Id = "user-123",
            FullName = "Jane Smith",
            UserName = "janesmith",
            Email = "jane@example.com",
            PhoneNumber = "9876543210",
            CreatedDate = createdDate,
            ModifiedDate = modifiedDate,
            IsActive = true,
            EmailConfirmed = true
        };

        // Assert
        Assert.Equal("user-123", user.Id);
        Assert.Equal("Jane Smith", user.FullName);
        Assert.Equal("janesmith", user.UserName);
        Assert.Equal("jane@example.com", user.Email);
        Assert.Equal("9876543210", user.PhoneNumber);
        Assert.Equal(createdDate, user.CreatedDate);
        Assert.Equal(modifiedDate, user.ModifiedDate);
        Assert.True(user.IsActive);
        Assert.True(user.EmailConfirmed);
    }

    [Theory]
    [InlineData("Alice Johnson")]
    [InlineData("Bob Smith")]
    [InlineData("Charlie Brown")]
    public void FullName_ShouldAcceptVariousNames(string fullName)
    {
        // Arrange
        var user = new User();

        // Act
        user.FullName = fullName;

        // Assert
        Assert.Equal(fullName, user.FullName);
    }

    [Fact]
    public void IsActive_ShouldDefaultToFalse()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        Assert.False(user.IsActive);
    }

    [Fact]
    public void User_ShouldAllowEmailConfirmationFlag()
    {
        // Arrange & Act
        var user = new User { EmailConfirmed = true };

        // Assert
        Assert.True(user.EmailConfirmed);
    }
}
