using Xunit;
using DiagnosticCenter.Domain.Entities;
using System;
using System.Collections.Generic;

namespace DiagnosticCenter.UnitTests.Entities;

public class TestTypeTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var testType = new TestType();

        // Assert
        Assert.Equal(0, testType.Id);
        Assert.Equal(string.Empty, testType.Name);
        Assert.Null(testType.Description);
        Assert.False(testType.IsActive);
        Assert.Equal(string.Empty, testType.CreatedBy);
        Assert.Null(testType.ModifiedBy);
        Assert.NotNull(testType.TestSetups);
        Assert.Empty(testType.TestSetups);
    }

    [Fact]
    public void Id_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var testType = new TestType();
        var expectedId = 456;

        // Act
        testType.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, testType.Id);
    }

    [Fact]
    public void Name_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var testType = new TestType();
        var expectedName = "Laboratory Test";

        // Act
        testType.Name = expectedName;

        // Assert
        Assert.Equal(expectedName, testType.Name);
    }

    [Fact]
    public void Description_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var testType = new TestType();
        var expectedDescription = "Blood and urine laboratory tests";

        // Act
        testType.Description = expectedDescription;

        // Assert
        Assert.Equal(expectedDescription, testType.Description);
    }

    [Fact]
    public void Description_ShouldAcceptNull()
    {
        // Arrange
        var testType = new TestType { Description = "Test description" };

        // Act
        testType.Description = null;

        // Assert
        Assert.Null(testType.Description);
    }

    [Fact]
    public void CreatedDate_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var testType = new TestType();
        var expectedDate = DateTime.Now;

        // Act
        testType.CreatedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, testType.CreatedDate);
    }

    [Fact]
    public void ModifiedDate_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var testType = new TestType();
        var expectedDate = DateTime.Now;

        // Act
        testType.ModifiedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, testType.ModifiedDate);
    }

    [Fact]
    public void ModifiedDate_ShouldAcceptNull()
    {
        // Arrange
        var testType = new TestType { ModifiedDate = DateTime.Now };

        // Act
        testType.ModifiedDate = null;

        // Assert
        Assert.Null(testType.ModifiedDate);
    }

    [Fact]
    public void IsActive_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var testType = new TestType();

        // Act
        testType.IsActive = true;

        // Assert
        Assert.True(testType.IsActive);
    }

    [Fact]
    public void CreatedBy_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var testType = new TestType();
        var expectedCreatedBy = "system@test.com";

        // Act
        testType.CreatedBy = expectedCreatedBy;

        // Assert
        Assert.Equal(expectedCreatedBy, testType.CreatedBy);
    }

    [Fact]
    public void ModifiedBy_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var testType = new TestType();
        var expectedModifiedBy = "admin@test.com";

        // Act
        testType.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedModifiedBy, testType.ModifiedBy);
    }

    [Fact]
    public void ModifiedBy_ShouldAcceptNull()
    {
        // Arrange
        var testType = new TestType { ModifiedBy = "user@test.com" };

        // Act
        testType.ModifiedBy = null;

        // Assert
        Assert.Null(testType.ModifiedBy);
    }

    [Fact]
    public void TestSetups_ShouldSetAndGetCollection()
    {
        // Arrange
        var testType = new TestType();
        var testSetups = new List<TestSetup>
        {
            new TestSetup { Id = 1, Name = "Setup 1" },
            new TestSetup { Id = 2, Name = "Setup 2" }
        };

        // Act
        testType.TestSetups = testSetups;

        // Assert
        Assert.NotNull(testType.TestSetups);
        Assert.Equal(2, testType.TestSetups.Count);
    }

    [Fact]
    public void TestType_ShouldAllowCompleteObjectInitialization()
    {
        // Arrange
        var createdDate = DateTime.Now;
        var modifiedDate = DateTime.Now.AddHours(2);

        // Act
        var testType = new TestType
        {
            Id = 200,
            Name = "Imaging Tests",
            Description = "X-Ray, CT Scan, MRI",
            CreatedDate = createdDate,
            ModifiedDate = modifiedDate,
            IsActive = true,
            CreatedBy = "admin",
            ModifiedBy = "supervisor",
            TestSetups = new List<TestSetup>
            {
                new TestSetup { Id = 1, Name = "X-Ray" }
            }
        };

        // Assert
        Assert.Equal(200, testType.Id);
        Assert.Equal("Imaging Tests", testType.Name);
        Assert.Equal("X-Ray, CT Scan, MRI", testType.Description);
        Assert.Equal(createdDate, testType.CreatedDate);
        Assert.Equal(modifiedDate, testType.ModifiedDate);
        Assert.True(testType.IsActive);
        Assert.Equal("admin", testType.CreatedBy);
        Assert.Equal("supervisor", testType.ModifiedBy);
        Assert.Single(testType.TestSetups);
    }
}
