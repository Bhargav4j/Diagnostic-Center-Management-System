using Xunit;
using DiagnosticCenter.Domain.Entities;
using System;
using System.Collections.Generic;

namespace DiagnosticCenter.UnitTests.Entities;

public class TestSetupTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var testSetup = new TestSetup();

        // Assert
        Assert.Equal(0, testSetup.Id);
        Assert.Equal(string.Empty, testSetup.Name);
        Assert.Equal(0, testSetup.Fee);
        Assert.Equal(0, testSetup.TypeId);
        Assert.False(testSetup.IsActive);
        Assert.Equal(string.Empty, testSetup.CreatedBy);
        Assert.Null(testSetup.ModifiedBy);
        Assert.Null(testSetup.Type);
        Assert.NotNull(testSetup.TestEntries);
        Assert.Empty(testSetup.TestEntries);
    }

    [Fact]
    public void Id_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var testSetup = new TestSetup();
        var expectedId = 123;

        // Act
        testSetup.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, testSetup.Id);
    }

    [Fact]
    public void Name_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var testSetup = new TestSetup();
        var expectedName = "Blood Test";

        // Act
        testSetup.Name = expectedName;

        // Assert
        Assert.Equal(expectedName, testSetup.Name);
    }

    [Fact]
    public void Fee_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var testSetup = new TestSetup();
        var expectedFee = 150.50m;

        // Act
        testSetup.Fee = expectedFee;

        // Assert
        Assert.Equal(expectedFee, testSetup.Fee);
    }

    [Fact]
    public void TypeId_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var testSetup = new TestSetup();
        var expectedTypeId = 5;

        // Act
        testSetup.TypeId = expectedTypeId;

        // Assert
        Assert.Equal(expectedTypeId, testSetup.TypeId);
    }

    [Fact]
    public void CreatedDate_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var testSetup = new TestSetup();
        var expectedDate = DateTime.Now;

        // Act
        testSetup.CreatedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, testSetup.CreatedDate);
    }

    [Fact]
    public void ModifiedDate_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var testSetup = new TestSetup();
        var expectedDate = DateTime.Now;

        // Act
        testSetup.ModifiedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, testSetup.ModifiedDate);
    }

    [Fact]
    public void ModifiedDate_ShouldAcceptNull()
    {
        // Arrange
        var testSetup = new TestSetup { ModifiedDate = DateTime.Now };

        // Act
        testSetup.ModifiedDate = null;

        // Assert
        Assert.Null(testSetup.ModifiedDate);
    }

    [Fact]
    public void IsActive_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var testSetup = new TestSetup();

        // Act
        testSetup.IsActive = true;

        // Assert
        Assert.True(testSetup.IsActive);
    }

    [Fact]
    public void CreatedBy_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var testSetup = new TestSetup();
        var expectedCreatedBy = "admin@test.com";

        // Act
        testSetup.CreatedBy = expectedCreatedBy;

        // Assert
        Assert.Equal(expectedCreatedBy, testSetup.CreatedBy);
    }

    [Fact]
    public void ModifiedBy_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var testSetup = new TestSetup();
        var expectedModifiedBy = "user@test.com";

        // Act
        testSetup.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedModifiedBy, testSetup.ModifiedBy);
    }

    [Fact]
    public void Type_ShouldSetAndGetNavigationProperty()
    {
        // Arrange
        var testSetup = new TestSetup();
        var testType = new TestType { Id = 1, Name = "Lab Test" };

        // Act
        testSetup.Type = testType;

        // Assert
        Assert.NotNull(testSetup.Type);
        Assert.Equal(testType.Id, testSetup.Type.Id);
        Assert.Equal(testType.Name, testSetup.Type.Name);
    }

    [Fact]
    public void TestEntries_ShouldSetAndGetCollection()
    {
        // Arrange
        var testSetup = new TestSetup();
        var testEntries = new List<TestEntry>
        {
            new TestEntry { Id = 1 },
            new TestEntry { Id = 2 }
        };

        // Act
        testSetup.TestEntries = testEntries;

        // Assert
        Assert.NotNull(testSetup.TestEntries);
        Assert.Equal(2, testSetup.TestEntries.Count);
    }

    [Fact]
    public void TestSetup_ShouldAllowCompleteObjectInitialization()
    {
        // Arrange
        var createdDate = DateTime.Now;
        var modifiedDate = DateTime.Now.AddDays(1);

        // Act
        var testSetup = new TestSetup
        {
            Id = 100,
            Name = "Complete Blood Count",
            Fee = 250.75m,
            TypeId = 10,
            CreatedDate = createdDate,
            ModifiedDate = modifiedDate,
            IsActive = true,
            CreatedBy = "admin",
            ModifiedBy = "editor",
            Type = new TestType { Id = 10, Name = "Blood Work" }
        };

        // Assert
        Assert.Equal(100, testSetup.Id);
        Assert.Equal("Complete Blood Count", testSetup.Name);
        Assert.Equal(250.75m, testSetup.Fee);
        Assert.Equal(10, testSetup.TypeId);
        Assert.Equal(createdDate, testSetup.CreatedDate);
        Assert.Equal(modifiedDate, testSetup.ModifiedDate);
        Assert.True(testSetup.IsActive);
        Assert.Equal("admin", testSetup.CreatedBy);
        Assert.Equal("editor", testSetup.ModifiedBy);
        Assert.NotNull(testSetup.Type);
    }
}
