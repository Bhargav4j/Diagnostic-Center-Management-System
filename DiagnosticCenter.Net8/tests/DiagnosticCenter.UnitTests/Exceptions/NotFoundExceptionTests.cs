using Xunit;
using DiagnosticCenter.Domain.Exceptions;
using System;

namespace DiagnosticCenter.UnitTests.Exceptions;

public class NotFoundExceptionTests
{
    [Fact]
    public void Constructor_WithMessage_ShouldSetMessageCorrectly()
    {
        // Arrange
        var expectedMessage = "Entity not found";

        // Act
        var exception = new NotFoundException(expectedMessage);

        // Assert
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public void Constructor_WithMessage_ShouldInheritFromException()
    {
        // Arrange
        var message = "Test exception";

        // Act
        var exception = new NotFoundException(message);

        // Assert
        Assert.IsAssignableFrom<Exception>(exception);
    }

    [Fact]
    public void Constructor_WithEntityNameAndKey_ShouldFormatMessageCorrectly()
    {
        // Arrange
        var entityName = "TestSetup";
        var key = 123;
        var expectedMessage = $"Entity '{entityName}' with key '{key}' was not found.";

        // Act
        var exception = new NotFoundException(entityName, key);

        // Assert
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public void Constructor_WithEntityNameAndKey_ShouldHandleStringKey()
    {
        // Arrange
        var entityName = "User";
        var key = "user-123";
        var expectedMessage = $"Entity '{entityName}' with key '{key}' was not found.";

        // Act
        var exception = new NotFoundException(entityName, key);

        // Assert
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public void Constructor_WithEntityNameAndKey_ShouldHandleGuidKey()
    {
        // Arrange
        var entityName = "Payment";
        var key = Guid.NewGuid();
        var expectedMessage = $"Entity '{entityName}' with key '{key}' was not found.";

        // Act
        var exception = new NotFoundException(entityName, key);

        // Assert
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public void NotFoundException_ShouldBeThrowableAndCatchable()
    {
        // Arrange
        var message = "Entity not found";

        // Act
        var exception = new NotFoundException(message);

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public void NotFoundException_ShouldBeCatchableAsException()
    {
        // Arrange
        var message = "Entity not found";
        Exception caughtException = null;

        // Act
        try
        {
            throw new NotFoundException(message);
        }
        catch (Exception ex)
        {
            caughtException = ex;
        }

        // Assert
        Assert.NotNull(caughtException);
        Assert.IsType<NotFoundException>(caughtException);
        Assert.Equal(message, caughtException.Message);
    }

    [Theory]
    [InlineData("TestSetup", 1)]
    [InlineData("TestType", 999)]
    [InlineData("Payment", 0)]
    public void Constructor_WithEntityNameAndIntKey_ShouldFormatMessageCorrectly(string entityName, int key)
    {
        // Arrange
        var expectedMessage = $"Entity '{entityName}' with key '{key}' was not found.";

        // Act
        var exception = new NotFoundException(entityName, key);

        // Assert
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Theory]
    [InlineData("")]
    public void Constructor_WithMessage_ShouldHandleEmptyMessage(string message)
    {
        // Act
        var exception = new NotFoundException(message);

        // Assert
        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public void Constructor_WithEntityNameAndKey_ShouldHandleZeroKey()
    {
        // Arrange
        var entityName = "TestEntry";
        var key = 0;
        var expectedMessage = $"Entity '{entityName}' with key '{key}' was not found.";

        // Act
        var exception = new NotFoundException(entityName, key);

        // Assert
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public void NotFoundException_ShouldHaveDefaultExceptionProperties()
    {
        // Arrange
        var message = "Test not found";

        // Act
        var exception = new NotFoundException(message);

        // Assert
        Assert.Null(exception.InnerException);
        Assert.Equal(message, exception.Message);
    }
}
