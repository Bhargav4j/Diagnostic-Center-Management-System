using Xunit;
using DiagnosticCenter.Domain.Entities;

namespace Tests.DiagnosticCenter.Domain.Entities;

public class TestEntryItemTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange & Act
        var testEntryItem = new TestEntryItem();

        // Assert
        Assert.Equal(0, testEntryItem.TestEntryId);
        Assert.Equal(0, testEntryItem.TestSetupId);
        Assert.Equal(0, testEntryItem.Fee);
        Assert.Null(testEntryItem.TestEntry);
        Assert.Null(testEntryItem.TestSetup);
    }

    [Fact]
    public void TestEntryId_ShouldSetAndGetValue()
    {
        // Arrange
        var testEntryItem = new TestEntryItem();
        var expectedId = 123;

        // Act
        testEntryItem.TestEntryId = expectedId;

        // Assert
        Assert.Equal(expectedId, testEntryItem.TestEntryId);
    }

    [Fact]
    public void TestSetupId_ShouldSetAndGetValue()
    {
        // Arrange
        var testEntryItem = new TestEntryItem();
        var expectedId = 456;

        // Act
        testEntryItem.TestSetupId = expectedId;

        // Assert
        Assert.Equal(expectedId, testEntryItem.TestSetupId);
    }

    [Fact]
    public void Fee_ShouldSetAndGetValue()
    {
        // Arrange
        var testEntryItem = new TestEntryItem();
        var expectedFee = 150.75m;

        // Act
        testEntryItem.Fee = expectedFee;

        // Assert
        Assert.Equal(expectedFee, testEntryItem.Fee);
    }

    [Fact]
    public void Fee_ShouldHandleZeroValue()
    {
        // Arrange
        var testEntryItem = new TestEntryItem();

        // Act
        testEntryItem.Fee = 0m;

        // Assert
        Assert.Equal(0m, testEntryItem.Fee);
    }

    [Fact]
    public void TestEntry_ShouldSetAndGetValue()
    {
        // Arrange
        var testEntryItem = new TestEntryItem();
        var testEntry = new TestEntry { PatientName = "John Doe" };

        // Act
        testEntryItem.TestEntry = testEntry;

        // Assert
        Assert.Equal(testEntry, testEntryItem.TestEntry);
    }

    [Fact]
    public void TestSetup_ShouldSetAndGetValue()
    {
        // Arrange
        var testEntryItem = new TestEntryItem();
        var testSetup = new TestSetup { Name = "CBC Test" };

        // Act
        testEntryItem.TestSetup = testSetup;

        // Assert
        Assert.Equal(testSetup, testEntryItem.TestSetup);
    }

    [Fact]
    public void InheritsFromBaseEntity_ShouldHaveBaseEntityProperties()
    {
        // Arrange & Act
        var testEntryItem = new TestEntryItem();

        // Assert
        Assert.IsAssignableFrom<BaseEntity>(testEntryItem);
        Assert.Equal(0, testEntryItem.Id);
    }
}
