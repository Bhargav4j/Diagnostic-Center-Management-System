using Xunit;
using DiagnosticCenter.Domain.Entities;
using System;
using System.Collections.Generic;

namespace DiagnosticCenter.Domain.Entities.Tests;

public class TestTypeTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange & Act
        var testType = new TestType();

        // Assert
        Assert.NotNull(testType);
        Assert.Equal(string.Empty, testType.Name);
        Assert.Null(testType.Description);
        Assert.Equal(string.Empty, testType.CreatedBy);
        Assert.Null(testType.ModifiedBy);
        Assert.NotNull(testType.TestSetups);
    }

    [Fact]
    public void Id_SetAndGet_ShouldWork()
    {
        // Arrange
        var testType = new TestType();
        var expectedId = 123;

        // Act
        testType.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, testType.Id);
    }

    [Fact]
    public void Name_SetAndGet_ShouldWork()
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
    public void Description_SetAndGet_ShouldWork()
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
    public void CreatedDate_SetAndGet_ShouldWork()
    {
        // Arrange
        var testType = new TestType();
        var expectedDate = DateTime.UtcNow;

        // Act
        testType.CreatedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, testType.CreatedDate);
    }

    [Fact]
    public void ModifiedDate_SetAndGet_ShouldWork()
    {
        // Arrange
        var testType = new TestType();
        var expectedDate = DateTime.UtcNow;

        // Act
        testType.ModifiedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, testType.ModifiedDate);
    }

    [Fact]
    public void IsActive_SetAndGet_ShouldWork()
    {
        // Arrange
        var testType = new TestType();

        // Act
        testType.IsActive = true;

        // Assert
        Assert.True(testType.IsActive);
    }

    [Fact]
    public void IsActive_SetFalse_ShouldWork()
    {
        // Arrange
        var testType = new TestType();

        // Act
        testType.IsActive = false;

        // Assert
        Assert.False(testType.IsActive);
    }

    [Fact]
    public void CreatedBy_SetAndGet_ShouldWork()
    {
        // Arrange
        var testType = new TestType();
        var expectedCreatedBy = "Admin";

        // Act
        testType.CreatedBy = expectedCreatedBy;

        // Assert
        Assert.Equal(expectedCreatedBy, testType.CreatedBy);
    }

    [Fact]
    public void ModifiedBy_SetAndGet_ShouldWork()
    {
        // Arrange
        var testType = new TestType();
        var expectedModifiedBy = "Admin";

        // Act
        testType.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedModifiedBy, testType.ModifiedBy);
    }

    [Fact]
    public void TestSetups_NavigationProperty_ShouldBeInitialized()
    {
        // Arrange
        var testType = new TestType();

        // Assert
        Assert.NotNull(testType.TestSetups);
        Assert.Empty(testType.TestSetups);
    }

    [Fact]
    public void TestSetups_CanAddItems_ShouldWork()
    {
        // Arrange
        var testType = new TestType();
        var testSetup = new TestSetup { Id = 1, TestName = "Test Setup 1" };

        // Act
        testType.TestSetups.Add(testSetup);

        // Assert
        Assert.Single(testType.TestSetups);
        Assert.Contains(testSetup, testType.TestSetups);
    }

    [Fact]
    public void AllProperties_SetAndGet_ShouldWork()
    {
        // Arrange
        var testType = new TestType
        {
            Id = 1,
            Name = "X-Ray Test",
            Description = "Chest X-Ray",
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            IsActive = true,
            CreatedBy = "Admin",
            ModifiedBy = "Admin"
        };

        // Assert
        Assert.Equal(1, testType.Id);
        Assert.Equal("X-Ray Test", testType.Name);
        Assert.Equal("Chest X-Ray", testType.Description);
        Assert.True(testType.IsActive);
        Assert.Equal("Admin", testType.CreatedBy);
        Assert.Equal("Admin", testType.ModifiedBy);
    }
}
