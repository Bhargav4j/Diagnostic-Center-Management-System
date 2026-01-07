using Xunit;
using DiagnosticCenter.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Tests.DiagnosticCenter.Domain.Entities;

public class UserTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        Assert.Equal(string.Empty, user.FullName);
        Assert.Equal(string.Empty, user.AccountType);
        Assert.Equal(default(DateTime), user.CreatedDate);
        Assert.Null(user.ModifiedDate);
        Assert.False(user.IsActive);
    }

    [Fact]
    public void FullName_ShouldSetAndGetValue()
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
    public void AccountType_ShouldSetAndGetValue()
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
    public void CreatedDate_ShouldSetAndGetValue()
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
    public void ModifiedDate_ShouldSetAndGetValue()
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
    public void IsActive_ShouldSetAndGetValue()
    {
        // Arrange
        var user = new User();

        // Act
        user.IsActive = true;

        // Assert
        Assert.True(user.IsActive);
    }

    [Fact]
    public void InheritsFromIdentityUser_ShouldHaveIdentityUserProperties()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        Assert.IsAssignableFrom<IdentityUser>(user);
        Assert.Null(user.UserName);
        Assert.Null(user.Email);
    }

    [Fact]
    public void UserName_ShouldSetAndGetValue()
    {
        // Arrange
        var user = new User();
        var expectedUserName = "johndoe";

        // Act
        user.UserName = expectedUserName;

        // Assert
        Assert.Equal(expectedUserName, user.UserName);
    }

    [Fact]
    public void Email_ShouldSetAndGetValue()
    {
        // Arrange
        var user = new User();
        var expectedEmail = "john@example.com";

        // Act
        user.Email = expectedEmail;

        // Assert
        Assert.Equal(expectedEmail, user.Email);
    }
}
