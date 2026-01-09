using Xunit;
using DiagnosticCenter.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiagnosticCenter.UnitTests.Exceptions;

public class ValidationExceptionTests
{
    [Fact]
    public void Constructor_Default_ShouldSetDefaultMessage()
    {
        // Arrange & Act
        var exception = new ValidationException();

        // Assert
        Assert.Equal("One or more validation failures have occurred.", exception.Message);
    }

    [Fact]
    public void Constructor_Default_ShouldInitializeEmptyErrorsDictionary()
    {
        // Arrange & Act
        var exception = new ValidationException();

        // Assert
        Assert.NotNull(exception.Errors);
        Assert.Empty(exception.Errors);
    }

    [Fact]
    public void Constructor_WithErrors_ShouldSetErrorsCorrectly()
    {
        // Arrange
        var errors = new Dictionary<string, string[]>
        {
            { "Name", new[] { "Name is required" } },
            { "Email", new[] { "Email is invalid" } }
        };

        // Act
        var exception = new ValidationException(errors);

        // Assert
        Assert.NotNull(exception.Errors);
        Assert.Equal(2, exception.Errors.Count);
        Assert.True(exception.Errors.ContainsKey("Name"));
        Assert.True(exception.Errors.ContainsKey("Email"));
    }

    [Fact]
    public void Constructor_WithErrors_ShouldSetDefaultMessage()
    {
        // Arrange
        var errors = new Dictionary<string, string[]>
        {
            { "Field", new[] { "Error message" } }
        };

        // Act
        var exception = new ValidationException(errors);

        // Assert
        Assert.Equal("One or more validation failures have occurred.", exception.Message);
    }

    [Fact]
    public void Constructor_WithErrors_ShouldPreserveMultipleErrorsPerField()
    {
        // Arrange
        var errors = new Dictionary<string, string[]>
        {
            { "Password", new[] { "Password is required", "Password must be at least 8 characters", "Password must contain a number" } }
        };

        // Act
        var exception = new ValidationException(errors);

        // Assert
        Assert.Single(exception.Errors);
        Assert.Equal(3, exception.Errors["Password"].Length);
        Assert.Contains("Password is required", exception.Errors["Password"]);
        Assert.Contains("Password must be at least 8 characters", exception.Errors["Password"]);
        Assert.Contains("Password must contain a number", exception.Errors["Password"]);
    }

    [Fact]
    public void ValidationException_ShouldInheritFromException()
    {
        // Arrange & Act
        var exception = new ValidationException();

        // Assert
        Assert.IsAssignableFrom<Exception>(exception);
    }

    [Fact]
    public void ValidationException_ShouldBeThrowableAndCatchable()
    {
        // Arrange
        var errors = new Dictionary<string, string[]>
        {
            { "TestField", new[] { "Test error" } }
        };

        // Act
        var exception = new ValidationException(errors);

        // Assert
        Assert.NotNull(exception);
        Assert.NotNull(exception.Errors);
        Assert.Single(exception.Errors);
    }

    [Fact]
    public void ValidationException_ShouldBeCatchableAsException()
    {
        // Arrange
        Exception caughtException = null;

        // Act
        try
        {
            throw new ValidationException();
        }
        catch (Exception ex)
        {
            caughtException = ex;
        }

        // Assert
        Assert.NotNull(caughtException);
        Assert.IsType<ValidationException>(caughtException);
    }

    [Fact]
    public void Errors_ShouldBeReadOnly()
    {
        // Arrange
        var errors = new Dictionary<string, string[]>
        {
            { "Field1", new[] { "Error1" } }
        };
        var exception = new ValidationException(errors);

        // Act & Assert
        Assert.NotNull(exception.Errors);
        // The Errors property is a getter-only property
        Assert.Equal(errors, exception.Errors);
    }

    [Fact]
    public void Constructor_WithEmptyErrorsDictionary_ShouldAccept()
    {
        // Arrange
        var errors = new Dictionary<string, string[]>();

        // Act
        var exception = new ValidationException(errors);

        // Assert
        Assert.NotNull(exception.Errors);
        Assert.Empty(exception.Errors);
    }

    [Fact]
    public void Constructor_WithMultipleFields_ShouldPreserveAllErrors()
    {
        // Arrange
        var errors = new Dictionary<string, string[]>
        {
            { "FirstName", new[] { "First name is required" } },
            { "LastName", new[] { "Last name is required" } },
            { "Email", new[] { "Email is required", "Email format is invalid" } },
            { "PhoneNumber", new[] { "Phone number must be 10 digits" } }
        };

        // Act
        var exception = new ValidationException(errors);

        // Assert
        Assert.Equal(4, exception.Errors.Count);
        Assert.Single(exception.Errors["FirstName"]);
        Assert.Single(exception.Errors["LastName"]);
        Assert.Equal(2, exception.Errors["Email"].Length);
        Assert.Single(exception.Errors["PhoneNumber"]);
    }

    [Fact]
    public void ValidationException_ShouldHaveDefaultExceptionProperties()
    {
        // Arrange & Act
        var exception = new ValidationException();

        // Assert
        Assert.Null(exception.InnerException);
        Assert.NotNull(exception.Errors);
        Assert.Empty(exception.Errors);
    }

    [Theory]
    [InlineData("Name")]
    [InlineData("Email")]
    [InlineData("PhoneNumber")]
    public void Errors_ShouldContainSpecifiedFieldKey(string fieldName)
    {
        // Arrange
        var errors = new Dictionary<string, string[]>
        {
            { fieldName, new[] { "Error message" } }
        };

        // Act
        var exception = new ValidationException(errors);

        // Assert
        Assert.True(exception.Errors.ContainsKey(fieldName));
    }

    [Fact]
    public void Errors_ShouldReturnCorrectErrorArrayForField()
    {
        // Arrange
        var expectedErrors = new[] { "Error 1", "Error 2", "Error 3" };
        var errors = new Dictionary<string, string[]>
        {
            { "TestField", expectedErrors }
        };

        // Act
        var exception = new ValidationException(errors);

        // Assert
        Assert.Equal(expectedErrors, exception.Errors["TestField"]);
    }
}
