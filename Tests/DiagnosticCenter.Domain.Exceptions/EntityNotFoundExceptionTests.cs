using Xunit;
using DiagnosticCenter.Domain.Exceptions;

namespace Tests.DiagnosticCenter.Domain.Exceptions;

public class EntityNotFoundExceptionTests
{
    [Fact]
    public void Constructor_WithEntityNameAndId_ShouldCreateExceptionWithCorrectMessage()
    {
        // Arrange
        var entityName = "TestType";
        var id = 123;
        var expectedMessage = "TestType with ID 123 was not found.";

        // Act
        var exception = new EntityNotFoundException(entityName, id);

        // Assert
        Assert.Equal(expectedMessage, exception.Message);
        Assert.IsAssignableFrom<Exception>(exception);
    }

    [Fact]
    public void Constructor_WithEntityNameAndIdentifier_ShouldCreateExceptionWithCorrectMessage()
    {
        // Arrange
        var entityName = "TestType";
        var identifier = "BLOOD-TEST-001";
        var expectedMessage = "TestType with identifier 'BLOOD-TEST-001' was not found.";

        // Act
        var exception = new EntityNotFoundException(entityName, identifier);

        // Assert
        Assert.Equal(expectedMessage, exception.Message);
        Assert.IsAssignableFrom<Exception>(exception);
    }

    [Fact]
    public void Constructor_WithZeroId_ShouldCreateExceptionWithCorrectMessage()
    {
        // Arrange
        var entityName = "Payment";
        var id = 0;
        var expectedMessage = "Payment with ID 0 was not found.";

        // Act
        var exception = new EntityNotFoundException(entityName, id);

        // Assert
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public void Constructor_WithNegativeId_ShouldCreateExceptionWithCorrectMessage()
    {
        // Arrange
        var entityName = "TestSetup";
        var id = -1;
        var expectedMessage = "TestSetup with ID -1 was not found.";

        // Act
        var exception = new EntityNotFoundException(entityName, id);

        // Assert
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public void Constructor_WithEmptyIdentifier_ShouldCreateExceptionWithCorrectMessage()
    {
        // Arrange
        var entityName = "TestEntry";
        var identifier = "";
        var expectedMessage = "TestEntry with identifier '' was not found.";

        // Act
        var exception = new EntityNotFoundException(entityName, identifier);

        // Assert
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public void Exception_CanBeThrown()
    {
        // Arrange
        var entityName = "TestType";
        var id = 999;

        // Act & Assert
        Action act = () => throw new EntityNotFoundException(entityName, id);
        var exception = Assert.Throws<EntityNotFoundException>(act);

        Assert.NotNull(exception);
        Assert.Contains("TestType", exception.Message);
        Assert.Contains("999", exception.Message);
    }

    [Fact]
    public void Exception_CanBeCaught()
    {
        // Arrange
        var entityName = "Payment";
        var identifier = "PAY-123";
        var caught = false;

        // Act
        try
        {
            throw new EntityNotFoundException(entityName, identifier);
        }
        catch (EntityNotFoundException)
        {
            caught = true;
        }

        // Assert
        Assert.True(caught);
    }
}
