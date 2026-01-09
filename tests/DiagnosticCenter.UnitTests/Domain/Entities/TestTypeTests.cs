using DiagnosticCenter.Domain.Entities;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiagnosticCenter.UnitTests.Domain.Entities;

public class TestTypeTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties_WithDefaultValues()
    {
        // Arrange & Act
        var testType = new TestType();

        // Assert
        Assert.Equal(0, testType.Id);
        Assert.Equal(string.Empty, testType.Name);
        Assert.Equal(default(DateTime), testType.CreatedDate);
        Assert.Null(testType.ModifiedDate);
        Assert.True(testType.IsActive);
        Assert.Equal(string.Empty, testType.CreatedBy);
        Assert.Null(testType.ModifiedBy);
        Assert.NotNull(testType.TestSetups);
        Assert.Empty(testType.TestSetups);
    }

    [Fact]
    public void Id_ShouldSetAndGet_Correctly()
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
    public void Name_ShouldSetAndGet_Correctly()
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
    public void Name_ShouldAcceptEmptyString()
    {
        // Arrange
        var testType = new TestType();

        // Act
        testType.Name = string.Empty;

        // Assert
        Assert.Equal(string.Empty, testType.Name);
    }

    [Fact]
    public void CreatedDate_ShouldSetAndGet_Correctly()
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
    public void ModifiedDate_ShouldAcceptNullValue()
    {
        // Arrange
        var testType = new TestType();

        // Act
        testType.ModifiedDate = null;

        // Assert
        Assert.Null(testType.ModifiedDate);
    }

    [Fact]
    public void ModifiedDate_ShouldSetAndGet_Correctly()
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
    public void IsActive_ShouldDefaultToTrue()
    {
        // Arrange & Act
        var testType = new TestType();

        // Assert
        Assert.True(testType.IsActive);
    }

    [Fact]
    public void IsActive_ShouldSetToFalse()
    {
        // Arrange
        var testType = new TestType();

        // Act
        testType.IsActive = false;

        // Assert
        Assert.False(testType.IsActive);
    }

    [Fact]
    public void CreatedBy_ShouldSetAndGet_Correctly()
    {
        // Arrange
        var testType = new TestType();
        var expectedCreatedBy = "AdminUser";

        // Act
        testType.CreatedBy = expectedCreatedBy;

        // Assert
        Assert.Equal(expectedCreatedBy, testType.CreatedBy);
    }

    [Fact]
    public void ModifiedBy_ShouldAcceptNullValue()
    {
        // Arrange
        var testType = new TestType();

        // Act
        testType.ModifiedBy = null;

        // Assert
        Assert.Null(testType.ModifiedBy);
    }

    [Fact]
    public void ModifiedBy_ShouldSetAndGet_Correctly()
    {
        // Arrange
        var testType = new TestType();
        var expectedModifiedBy = "EditorUser";

        // Act
        testType.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedModifiedBy, testType.ModifiedBy);
    }

    [Fact]
    public void TestSetups_ShouldInitializeAsEmptyCollection()
    {
        // Arrange & Act
        var testType = new TestType();

        // Assert
        Assert.NotNull(testType.TestSetups);
        Assert.Empty(testType.TestSetups);
    }

    [Fact]
    public void TestSetups_ShouldAllowAddingItems()
    {
        // Arrange
        var testType = new TestType();
        var testSetup = new TestSetup { Id = 1 };

        // Act
        testType.TestSetups.Add(testSetup);

        // Assert
        Assert.Single(testType.TestSetups);
        Assert.Contains(testSetup, testType.TestSetups);
    }

    [Fact]
    public void TestSetups_ShouldAllowMultipleItems()
    {
        // Arrange
        var testType = new TestType();
        var testSetup1 = new TestSetup { Id = 1 };
        var testSetup2 = new TestSetup { Id = 2 };

        // Act
        testType.TestSetups.Add(testSetup1);
        testType.TestSetups.Add(testSetup2);

        // Assert
        Assert.Equal(2, testType.TestSetups.Count);
        Assert.Contains(testSetup1, testType.TestSetups);
        Assert.Contains(testSetup2, testType.TestSetups);
    }

    [Fact]
    public void AllProperties_ShouldSetAndGet_Correctly()
    {
        // Arrange
        var testType = new TestType();
        var expectedId = 100;
        var expectedName = "X-Ray";
        var expectedCreatedDate = new DateTime(2024, 1, 1);
        var expectedModifiedDate = new DateTime(2024, 2, 1);
        var expectedIsActive = false;
        var expectedCreatedBy = "Doctor1";
        var expectedModifiedBy = "Doctor2";

        // Act
        testType.Id = expectedId;
        testType.Name = expectedName;
        testType.CreatedDate = expectedCreatedDate;
        testType.ModifiedDate = expectedModifiedDate;
        testType.IsActive = expectedIsActive;
        testType.CreatedBy = expectedCreatedBy;
        testType.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedId, testType.Id);
        Assert.Equal(expectedName, testType.Name);
        Assert.Equal(expectedCreatedDate, testType.CreatedDate);
        Assert.Equal(expectedModifiedDate, testType.ModifiedDate);
        Assert.Equal(expectedIsActive, testType.IsActive);
        Assert.Equal(expectedCreatedBy, testType.CreatedBy);
        Assert.Equal(expectedModifiedBy, testType.ModifiedBy);
    }
}
