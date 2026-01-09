using DiagnosticCenter.Domain.Entities;
using Xunit;
using System;

namespace DiagnosticCenter.UnitTests.Domain.Entities;

public class UserTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties_WithDefaultValues()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        Assert.Equal(string.Empty, user.FullName);
        Assert.Equal(default(DateTime), user.CreatedDate);
        Assert.Null(user.ModifiedDate);
        Assert.True(user.IsActive);
    }

    [Fact]
    public void FullName_ShouldSetAndGet_Correctly()
    {
        // Arrange
        var user = new User();
        var expectedFullName = "John Smith";

        // Act
        user.FullName = expectedFullName;

        // Assert
        Assert.Equal(expectedFullName, user.FullName);
    }

    [Fact]
    public void FullName_ShouldAcceptEmptyString()
    {
        // Arrange
        var user = new User();

        // Act
        user.FullName = string.Empty;

        // Assert
        Assert.Equal(string.Empty, user.FullName);
    }

    [Fact]
    public void CreatedDate_ShouldSetAndGet_Correctly()
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
    public void CreatedDate_ShouldAcceptMinValue()
    {
        // Arrange
        var user = new User();

        // Act
        user.CreatedDate = DateTime.MinValue;

        // Assert
        Assert.Equal(DateTime.MinValue, user.CreatedDate);
    }

    [Fact]
    public void CreatedDate_ShouldAcceptMaxValue()
    {
        // Arrange
        var user = new User();

        // Act
        user.CreatedDate = DateTime.MaxValue;

        // Assert
        Assert.Equal(DateTime.MaxValue, user.CreatedDate);
    }

    [Fact]
    public void ModifiedDate_ShouldAcceptNullValue()
    {
        // Arrange
        var user = new User();

        // Act
        user.ModifiedDate = null;

        // Assert
        Assert.Null(user.ModifiedDate);
    }

    [Fact]
    public void ModifiedDate_ShouldSetAndGet_Correctly()
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
    public void IsActive_ShouldDefaultToTrue()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        Assert.True(user.IsActive);
    }

    [Fact]
    public void IsActive_ShouldSetToFalse()
    {
        // Arrange
        var user = new User();

        // Act
        user.IsActive = false;

        // Assert
        Assert.False(user.IsActive);
    }

    [Fact]
    public void IsActive_ShouldSetToTrue()
    {
        // Arrange
        var user = new User();
        user.IsActive = false;

        // Act
        user.IsActive = true;

        // Assert
        Assert.True(user.IsActive);
    }

    [Fact]
    public void AllProperties_ShouldSetAndGet_Correctly()
    {
        // Arrange
        var user = new User();
        var expectedFullName = "Jane Doe";
        var expectedCreatedDate = new DateTime(2024, 1, 1);
        var expectedModifiedDate = new DateTime(2024, 6, 15);
        var expectedIsActive = false;

        // Act
        user.FullName = expectedFullName;
        user.CreatedDate = expectedCreatedDate;
        user.ModifiedDate = expectedModifiedDate;
        user.IsActive = expectedIsActive;

        // Assert
        Assert.Equal(expectedFullName, user.FullName);
        Assert.Equal(expectedCreatedDate, user.CreatedDate);
        Assert.Equal(expectedModifiedDate, user.ModifiedDate);
        Assert.Equal(expectedIsActive, user.IsActive);
    }

    [Fact]
    public void User_ShouldInheritFromIdentityUser()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        Assert.IsAssignableFrom<Microsoft.AspNetCore.Identity.IdentityUser<int>>(user);
    }

    [Fact]
    public void Id_ShouldBeIntegerType()
    {
        // Arrange
        var user = new User();

        // Act
        user.Id = 123;

        // Assert
        Assert.Equal(123, user.Id);
        Assert.IsType<int>(user.Id);
    }

    [Fact]
    public void UserName_ShouldSetAndGet_FromIdentityUser()
    {
        // Arrange
        var user = new User();
        var expectedUserName = "jsmith@example.com";

        // Act
        user.UserName = expectedUserName;

        // Assert
        Assert.Equal(expectedUserName, user.UserName);
    }

    [Fact]
    public void Email_ShouldSetAndGet_FromIdentityUser()
    {
        // Arrange
        var user = new User();
        var expectedEmail = "jsmith@example.com";

        // Act
        user.Email = expectedEmail;

        // Assert
        Assert.Equal(expectedEmail, user.Email);
    }
}
