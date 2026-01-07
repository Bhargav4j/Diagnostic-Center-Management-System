using Xunit;
using DiagnosticCenter.Domain.Entities;

namespace Tests.DiagnosticCenter.Domain.Entities;

public class TestTypeTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange & Act
        var testType = new TestType();

        // Assert
        Assert.Equal(string.Empty, testType.Name);
        Assert.Null(testType.Description);
        Assert.NotNull(testType.TestSetups);
        Assert.Empty(testType.TestSetups);
    }

    [Fact]
    public void Name_ShouldSetAndGetValue()
    {
        // Arrange
        var testType = new TestType();
        var expectedName = "Blood Test";

        // Act
        testType.Name = expectedName;

        // Assert
        Assert.Equal(expectedName, testType.Name);
    }

    [Fact]
    public void Description_ShouldSetAndGetValue()
    {
        // Arrange
        var testType = new TestType();
        var expectedDescription = "Complete blood count test";

        // Act
        testType.Description = expectedDescription;

        // Assert
        Assert.Equal(expectedDescription, testType.Description);
    }

    [Fact]
    public void TestSetups_ShouldBeInitializedAsEmptyList()
    {
        // Arrange & Act
        var testType = new TestType();

        // Assert
        Assert.NotNull(testType.TestSetups);
        Assert.IsAssignableFrom<ICollection<TestSetup>>(testType.TestSetups);
        Assert.Empty(testType.TestSetups);
    }

    [Fact]
    public void TestSetups_ShouldAllowAddingTestSetups()
    {
        // Arrange
        var testType = new TestType();
        var testSetup = new TestSetup { Name = "CBC Test" };

        // Act
        testType.TestSetups.Add(testSetup);

        // Assert
        Assert.Single(testType.TestSetups);
        Assert.Contains(testSetup, testType.TestSetups);
    }

    [Fact]
    public void InheritsFromBaseEntity_ShouldHaveBaseEntityProperties()
    {
        // Arrange & Act
        var testType = new TestType();

        // Assert
        Assert.IsAssignableFrom<BaseEntity>(testType);
        Assert.Equal(0, testType.Id);
        Assert.Equal(default(DateTime), testType.CreatedDate);
    }
}
