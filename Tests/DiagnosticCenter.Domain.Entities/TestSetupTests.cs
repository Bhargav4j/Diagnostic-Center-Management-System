using Xunit;
using DiagnosticCenter.Domain.Entities;

namespace Tests.DiagnosticCenter.Domain.Entities;

public class TestSetupTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange & Act
        var testSetup = new TestSetup();

        // Assert
        Assert.Equal(string.Empty, testSetup.Name);
        Assert.Null(testSetup.Description);
        Assert.Equal(0, testSetup.Fee);
        Assert.Equal(0, testSetup.TestTypeId);
        Assert.Null(testSetup.TestType);
        Assert.NotNull(testSetup.TestEntryItems);
        Assert.Empty(testSetup.TestEntryItems);
    }

    [Fact]
    public void Name_ShouldSetAndGetValue()
    {
        // Arrange
        var testSetup = new TestSetup();
        var expectedName = "Complete Blood Count";

        // Act
        testSetup.Name = expectedName;

        // Assert
        Assert.Equal(expectedName, testSetup.Name);
    }

    [Fact]
    public void Description_ShouldSetAndGetValue()
    {
        // Arrange
        var testSetup = new TestSetup();
        var expectedDescription = "Full blood panel test";

        // Act
        testSetup.Description = expectedDescription;

        // Assert
        Assert.Equal(expectedDescription, testSetup.Description);
    }

    [Fact]
    public void Fee_ShouldSetAndGetValue()
    {
        // Arrange
        var testSetup = new TestSetup();
        var expectedFee = 99.99m;

        // Act
        testSetup.Fee = expectedFee;

        // Assert
        Assert.Equal(expectedFee, testSetup.Fee);
    }

    [Fact]
    public void Fee_ShouldHandleZeroValue()
    {
        // Arrange
        var testSetup = new TestSetup();

        // Act
        testSetup.Fee = 0m;

        // Assert
        Assert.Equal(0m, testSetup.Fee);
    }

    [Fact]
    public void TestTypeId_ShouldSetAndGetValue()
    {
        // Arrange
        var testSetup = new TestSetup();
        var expectedId = 123;

        // Act
        testSetup.TestTypeId = expectedId;

        // Assert
        Assert.Equal(expectedId, testSetup.TestTypeId);
    }

    [Fact]
    public void TestType_ShouldSetAndGetValue()
    {
        // Arrange
        var testSetup = new TestSetup();
        var testType = new TestType { Name = "Blood Test" };

        // Act
        testSetup.TestType = testType;

        // Assert
        Assert.Equal(testType, testSetup.TestType);
    }

    [Fact]
    public void TestEntryItems_ShouldBeInitializedAsEmptyList()
    {
        // Arrange & Act
        var testSetup = new TestSetup();

        // Assert
        Assert.NotNull(testSetup.TestEntryItems);
        Assert.IsAssignableFrom<ICollection<TestEntryItem>>(testSetup.TestEntryItems);
        Assert.Empty(testSetup.TestEntryItems);
    }

    [Fact]
    public void TestEntryItems_ShouldAllowAddingItems()
    {
        // Arrange
        var testSetup = new TestSetup();
        var item = new TestEntryItem { Fee = 100m };

        // Act
        testSetup.TestEntryItems.Add(item);

        // Assert
        Assert.Single(testSetup.TestEntryItems);
        Assert.Contains(item, testSetup.TestEntryItems);
    }

    [Fact]
    public void InheritsFromBaseEntity_ShouldHaveBaseEntityProperties()
    {
        // Arrange & Act
        var testSetup = new TestSetup();

        // Assert
        Assert.IsAssignableFrom<BaseEntity>(testSetup);
        Assert.Equal(0, testSetup.Id);
    }
}
