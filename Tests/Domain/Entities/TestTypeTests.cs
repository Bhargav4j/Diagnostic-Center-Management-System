using Xunit;
using DiagnosticCenter.Domain.Entities;

namespace DiagnosticCenterTests.Domain.Entities;

public class TestTypeTests
{
    [Fact]
    public void TestType_Constructor_SetsDefaultValues()
    {
        var testType = new TestType();

        Assert.Equal(0, testType.Id);
        Assert.Equal(string.Empty, testType.Name);
        Assert.True(testType.IsActive);
        Assert.Equal(string.Empty, testType.CreatedBy);
        Assert.Null(testType.ModifiedBy);
        Assert.Null(testType.ModifiedDate);
        Assert.NotNull(testType.TestSetups);
        Assert.Empty(testType.TestSetups);
    }

    [Fact]
    public void TestType_CreatedDate_IsSetToUtcNow()
    {
        var beforeCreation = DateTime.UtcNow;
        var testType = new TestType();
        var afterCreation = DateTime.UtcNow;

        Assert.True(testType.CreatedDate >= beforeCreation);
        Assert.True(testType.CreatedDate <= afterCreation);
    }

    [Fact]
    public void TestType_SetId_UpdatesIdProperty()
    {
        var testType = new TestType();
        testType.Id = 100;

        Assert.Equal(100, testType.Id);
    }

    [Fact]
    public void TestType_SetName_UpdatesNameProperty()
    {
        var testType = new TestType();
        testType.Name = "Blood Test";

        Assert.Equal("Blood Test", testType.Name);
    }

    [Fact]
    public void TestType_SetIsActive_UpdatesIsActiveProperty()
    {
        var testType = new TestType();
        testType.IsActive = false;

        Assert.False(testType.IsActive);
    }

    [Fact]
    public void TestType_TestSetups_CanAddItems()
    {
        var testType = new TestType();
        var testSetup = new TestSetup { Id = 1, TestTypeId = 1 };

        testType.TestSetups.Add(testSetup);

        Assert.Single(testType.TestSetups);
        Assert.Contains(testSetup, testType.TestSetups);
    }
}
