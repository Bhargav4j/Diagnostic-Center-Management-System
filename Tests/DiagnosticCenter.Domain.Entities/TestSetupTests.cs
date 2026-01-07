using Xunit;
using DiagnosticCenter.Domain.Entities;
using System;

namespace DiagnosticCenter.Domain.Entities.Tests;

public class TestSetupTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange & Act
        var testSetup = new TestSetup();

        // Assert
        Assert.NotNull(testSetup);
        Assert.Equal(string.Empty, testSetup.TestName);
        Assert.Equal(string.Empty, testSetup.CreatedBy);
        Assert.NotNull(testSetup.TestEntries);
    }

    [Fact]
    public void Id_SetAndGet_ShouldWork()
    {
        // Arrange
        var testSetup = new TestSetup();
        var expectedId = 456;

        // Act
        testSetup.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, testSetup.Id);
    }

    [Fact]
    public void TestTypeId_SetAndGet_ShouldWork()
    {
        // Arrange
        var testSetup = new TestSetup();
        var expectedTestTypeId = 1;

        // Act
        testSetup.TestTypeId = expectedTestTypeId;

        // Assert
        Assert.Equal(expectedTestTypeId, testSetup.TestTypeId);
    }

    [Fact]
    public void TestName_SetAndGet_ShouldWork()
    {
        // Arrange
        var testSetup = new TestSetup();
        var expectedTestName = "Complete Blood Count";

        // Act
        testSetup.TestName = expectedTestName;

        // Assert
        Assert.Equal(expectedTestName, testSetup.TestName);
    }

    [Fact]
    public void TestFee_SetAndGet_ShouldWork()
    {
        // Arrange
        var testSetup = new TestSetup();
        var expectedFee = 250.50m;

        // Act
        testSetup.TestFee = expectedFee;

        // Assert
        Assert.Equal(expectedFee, testSetup.TestFee);
    }

    [Fact]
    public void TestFee_SetZero_ShouldWork()
    {
        // Arrange
        var testSetup = new TestSetup();

        // Act
        testSetup.TestFee = 0m;

        // Assert
        Assert.Equal(0m, testSetup.TestFee);
    }

    [Fact]
    public void TestFee_SetNegative_ShouldWork()
    {
        // Arrange
        var testSetup = new TestSetup();

        // Act
        testSetup.TestFee = -100m;

        // Assert
        Assert.Equal(-100m, testSetup.TestFee);
    }

    [Fact]
    public void Description_SetAndGet_ShouldWork()
    {
        // Arrange
        var testSetup = new TestSetup();
        var expectedDescription = "Blood test for complete blood count";

        // Act
        testSetup.Description = expectedDescription;

        // Assert
        Assert.Equal(expectedDescription, testSetup.Description);
    }

    [Fact]
    public void CreatedDate_SetAndGet_ShouldWork()
    {
        // Arrange
        var testSetup = new TestSetup();
        var expectedDate = DateTime.UtcNow;

        // Act
        testSetup.CreatedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, testSetup.CreatedDate);
    }

    [Fact]
    public void IsActive_SetAndGet_ShouldWork()
    {
        // Arrange
        var testSetup = new TestSetup();

        // Act
        testSetup.IsActive = true;

        // Assert
        Assert.True(testSetup.IsActive);
    }

    [Fact]
    public void TestType_NavigationProperty_CanBeSet()
    {
        // Arrange
        var testSetup = new TestSetup();
        var testType = new TestType { Id = 1, Name = "Blood Test" };

        // Act
        testSetup.TestType = testType;

        // Assert
        Assert.NotNull(testSetup.TestType);
        Assert.Equal(testType, testSetup.TestType);
    }

    [Fact]
    public void TestEntries_NavigationProperty_ShouldBeInitialized()
    {
        // Arrange
        var testSetup = new TestSetup();

        // Assert
        Assert.NotNull(testSetup.TestEntries);
        Assert.Empty(testSetup.TestEntries);
    }

    [Fact]
    public void TestEntries_CanAddItems_ShouldWork()
    {
        // Arrange
        var testSetup = new TestSetup();
        var testEntry = new TestEntry { Id = 1, PatientName = "John Doe" };

        // Act
        testSetup.TestEntries.Add(testEntry);

        // Assert
        Assert.Single(testSetup.TestEntries);
        Assert.Contains(testEntry, testSetup.TestEntries);
    }

    [Fact]
    public void AllProperties_SetAndGet_ShouldWork()
    {
        // Arrange
        var testSetup = new TestSetup
        {
            Id = 1,
            TestTypeId = 2,
            TestName = "Blood Glucose Test",
            TestFee = 150.00m,
            Description = "Fasting blood glucose test",
            CreatedDate = DateTime.UtcNow,
            IsActive = true,
            CreatedBy = "Admin",
            ModifiedBy = "Admin"
        };

        // Assert
        Assert.Equal(1, testSetup.Id);
        Assert.Equal(2, testSetup.TestTypeId);
        Assert.Equal("Blood Glucose Test", testSetup.TestName);
        Assert.Equal(150.00m, testSetup.TestFee);
        Assert.Equal("Fasting blood glucose test", testSetup.Description);
        Assert.True(testSetup.IsActive);
    }
}
