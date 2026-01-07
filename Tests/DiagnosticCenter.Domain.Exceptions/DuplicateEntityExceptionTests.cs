using Xunit;
using DiagnosticCenter.Domain.Exceptions;

namespace Tests.DiagnosticCenter.Domain.Exceptions;

public class DuplicateEntityExceptionTests
{
    [Fact]
    public void Constructor_ShouldCreateExceptionWithCorrectMessage()
    {
        // Arrange
        var entityName = "TestType";
        var fieldName = "Name";
        var value = "Blood Test";
        var expectedMessage = "TestType with Name 'Blood Test' already exists.";

        // Act
        var exception = new DuplicateEntityException(entityName, fieldName, value);

        // Assert
        Assert.Equal(expectedMessage, exception.Message);
        Assert.IsAssignableFrom<Exception>(exception);
    }

    [Fact]
    public void Constructor_WithEmptyValue_ShouldCreateExceptionWithCorrectMessage()
    {
        // Arrange
        var entityName = "TestSetup";
        var fieldName = "Name";
        var value = "";
        var expectedMessage = "TestSetup with Name '' already exists.";

        // Act
        var exception = new DuplicateEntityException(entityName, fieldName, value);

        // Assert
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public void Constructor_WithDifferentFieldName_ShouldCreateExceptionWithCorrectMessage()
    {
        // Arrange
        var entityName = "User";
        var fieldName = "Email";
        var value = "test@example.com";
        var expectedMessage = "User with Email 'test@example.com' already exists.";

        // Act
        var exception = new DuplicateEntityException(entityName, fieldName, value);

        // Assert
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public void Exception_CanBeThrown()
    {
        // Arrange
        var entityName = "TestType";
        var fieldName = "Name";
        var value = "Duplicate Test";

        // Act & Assert
        Action act = () => throw new DuplicateEntityException(entityName, fieldName, value);
        var exception = Assert.Throws<DuplicateEntityException>(act);

        Assert.NotNull(exception);
        Assert.Contains("TestType", exception.Message);
        Assert.Contains("Name", exception.Message);
        Assert.Contains("Duplicate Test", exception.Message);
    }

    [Fact]
    public void Exception_CanBeCaught()
    {
        // Arrange
        var entityName = "Payment";
        var fieldName = "TransactionReference";
        var value = "TXN-123";
        var caught = false;

        // Act
        try
        {
            throw new DuplicateEntityException(entityName, fieldName, value);
        }
        catch (DuplicateEntityException)
        {
            caught = true;
        }

        // Assert
        Assert.True(caught);
    }

    [Fact]
    public void Exception_ShouldIncludeAllParametersInMessage()
    {
        // Arrange
        var entityName = "TestEntry";
        var fieldName = "BillNumber";
        var value = "BILL-2024-001";

        // Act
        var exception = new DuplicateEntityException(entityName, fieldName, value);

        // Assert
        Assert.Contains(entityName, exception.Message);
        Assert.Contains(fieldName, exception.Message);
        Assert.Contains(value, exception.Message);
    }
}
