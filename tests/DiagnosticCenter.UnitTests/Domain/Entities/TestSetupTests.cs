using DiagnosticCenter.Domain.Entities;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiagnosticCenter.UnitTests.Domain.Entities;

public class TestSetupTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties_WithDefaultValues()
    {
        // Arrange & Act
        var testSetup = new TestSetup();

        // Assert
        Assert.Equal(0, testSetup.Id);
        Assert.Equal(string.Empty, testSetup.Name);
        Assert.Equal(0m, testSetup.Fee);
        Assert.Equal(0, testSetup.TestTypeId);
        Assert.Equal(default(DateTime), testSetup.CreatedDate);
        Assert.Null(testSetup.ModifiedDate);
        Assert.True(testSetup.IsActive);
        Assert.Equal(string.Empty, testSetup.CreatedBy);
        Assert.Null(testSetup.ModifiedBy);
        Assert.Null(testSetup.TestType);
        Assert.NotNull(testSetup.PatientTests);
        Assert.Empty(testSetup.PatientTests);
    }

    [Fact]
    public void Id_ShouldSetAndGet_Correctly()
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
    public void Name_ShouldSetAndGet_Correctly()
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
    public void Name_ShouldAcceptEmptyString()
    {
        // Arrange
        var testSetup = new TestSetup();

        // Act
        testSetup.Name = string.Empty;

        // Assert
        Assert.Equal(string.Empty, testSetup.Name);
    }

    [Fact]
    public void Fee_ShouldSetAndGet_Correctly()
    {
        // Arrange
        var testSetup = new TestSetup();
        var expectedFee = 250.50m;

        // Act
        testSetup.Fee = expectedFee;

        // Assert
        Assert.Equal(expectedFee, testSetup.Fee);
    }

    [Fact]
    public void Fee_ShouldAcceptZeroValue()
    {
        // Arrange
        var testSetup = new TestSetup();

        // Act
        testSetup.Fee = 0m;

        // Assert
        Assert.Equal(0m, testSetup.Fee);
    }

    [Fact]
    public void Fee_ShouldAcceptNegativeValue()
    {
        // Arrange
        var testSetup = new TestSetup();

        // Act
        testSetup.Fee = -100m;

        // Assert
        Assert.Equal(-100m, testSetup.Fee);
    }

    [Fact]
    public void Fee_ShouldAcceptLargeValue()
    {
        // Arrange
        var testSetup = new TestSetup();
        var largeFee = 999999.99m;

        // Act
        testSetup.Fee = largeFee;

        // Assert
        Assert.Equal(largeFee, testSetup.Fee);
    }

    [Fact]
    public void TestTypeId_ShouldSetAndGet_Correctly()
    {
        // Arrange
        var testSetup = new TestSetup();
        var expectedTestTypeId = 10;

        // Act
        testSetup.TestTypeId = expectedTestTypeId;

        // Assert
        Assert.Equal(expectedTestTypeId, testSetup.TestTypeId);
    }

    [Fact]
    public void CreatedDate_ShouldSetAndGet_Correctly()
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
    public void ModifiedDate_ShouldAcceptNullValue()
    {
        // Arrange
        var testSetup = new TestSetup();

        // Act
        testSetup.ModifiedDate = null;

        // Assert
        Assert.Null(testSetup.ModifiedDate);
    }

    [Fact]
    public void ModifiedDate_ShouldSetAndGet_Correctly()
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
    public void IsActive_ShouldDefaultToTrue()
    {
        // Arrange & Act
        var testSetup = new TestSetup();

        // Assert
        Assert.True(testSetup.IsActive);
    }

    [Fact]
    public void IsActive_ShouldSetToFalse()
    {
        // Arrange
        var testSetup = new TestSetup();

        // Act
        testSetup.IsActive = false;

        // Assert
        Assert.False(testSetup.IsActive);
    }

    [Fact]
    public void CreatedBy_ShouldSetAndGet_Correctly()
    {
        // Arrange
        var testSetup = new TestSetup();
        var expectedCreatedBy = "AdminUser";

        // Act
        testSetup.CreatedBy = expectedCreatedBy;

        // Assert
        Assert.Equal(expectedCreatedBy, testSetup.CreatedBy);
    }

    [Fact]
    public void ModifiedBy_ShouldAcceptNullValue()
    {
        // Arrange
        var testSetup = new TestSetup();

        // Act
        testSetup.ModifiedBy = null;

        // Assert
        Assert.Null(testSetup.ModifiedBy);
    }

    [Fact]
    public void ModifiedBy_ShouldSetAndGet_Correctly()
    {
        // Arrange
        var testSetup = new TestSetup();
        var expectedModifiedBy = "EditorUser";

        // Act
        testSetup.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedModifiedBy, testSetup.ModifiedBy);
    }

    [Fact]
    public void TestType_ShouldAcceptNullValue()
    {
        // Arrange
        var testSetup = new TestSetup();

        // Act
        testSetup.TestType = null;

        // Assert
        Assert.Null(testSetup.TestType);
    }

    [Fact]
    public void TestType_ShouldSetAndGet_Correctly()
    {
        // Arrange
        var testSetup = new TestSetup();
        var testType = new TestType { Id = 1, Name = "Blood Test" };

        // Act
        testSetup.TestType = testType;

        // Assert
        Assert.NotNull(testSetup.TestType);
        Assert.Equal(testType, testSetup.TestType);
        Assert.Equal(1, testSetup.TestType.Id);
        Assert.Equal("Blood Test", testSetup.TestType.Name);
    }

    [Fact]
    public void PatientTests_ShouldInitializeAsEmptyCollection()
    {
        // Arrange & Act
        var testSetup = new TestSetup();

        // Assert
        Assert.NotNull(testSetup.PatientTests);
        Assert.Empty(testSetup.PatientTests);
    }

    [Fact]
    public void PatientTests_ShouldAllowAddingItems()
    {
        // Arrange
        var testSetup = new TestSetup();
        var patientTest = new PatientTest { Id = 1 };

        // Act
        testSetup.PatientTests.Add(patientTest);

        // Assert
        Assert.Single(testSetup.PatientTests);
        Assert.Contains(patientTest, testSetup.PatientTests);
    }

    [Fact]
    public void PatientTests_ShouldAllowMultipleItems()
    {
        // Arrange
        var testSetup = new TestSetup();
        var patientTest1 = new PatientTest { Id = 1 };
        var patientTest2 = new PatientTest { Id = 2 };

        // Act
        testSetup.PatientTests.Add(patientTest1);
        testSetup.PatientTests.Add(patientTest2);

        // Assert
        Assert.Equal(2, testSetup.PatientTests.Count);
        Assert.Contains(patientTest1, testSetup.PatientTests);
        Assert.Contains(patientTest2, testSetup.PatientTests);
    }

    [Fact]
    public void AllProperties_ShouldSetAndGet_Correctly()
    {
        // Arrange
        var testSetup = new TestSetup();
        var expectedId = 789;
        var expectedName = "Lipid Profile";
        var expectedFee = 500.75m;
        var expectedTestTypeId = 5;
        var expectedCreatedDate = new DateTime(2024, 1, 15);
        var expectedModifiedDate = new DateTime(2024, 3, 20);
        var expectedIsActive = false;
        var expectedCreatedBy = "Lab1";
        var expectedModifiedBy = "Lab2";
        var expectedTestType = new TestType { Id = 5, Name = "Blood Analysis" };

        // Act
        testSetup.Id = expectedId;
        testSetup.Name = expectedName;
        testSetup.Fee = expectedFee;
        testSetup.TestTypeId = expectedTestTypeId;
        testSetup.CreatedDate = expectedCreatedDate;
        testSetup.ModifiedDate = expectedModifiedDate;
        testSetup.IsActive = expectedIsActive;
        testSetup.CreatedBy = expectedCreatedBy;
        testSetup.ModifiedBy = expectedModifiedBy;
        testSetup.TestType = expectedTestType;

        // Assert
        Assert.Equal(expectedId, testSetup.Id);
        Assert.Equal(expectedName, testSetup.Name);
        Assert.Equal(expectedFee, testSetup.Fee);
        Assert.Equal(expectedTestTypeId, testSetup.TestTypeId);
        Assert.Equal(expectedCreatedDate, testSetup.CreatedDate);
        Assert.Equal(expectedModifiedDate, testSetup.ModifiedDate);
        Assert.Equal(expectedIsActive, testSetup.IsActive);
        Assert.Equal(expectedCreatedBy, testSetup.CreatedBy);
        Assert.Equal(expectedModifiedBy, testSetup.ModifiedBy);
        Assert.Equal(expectedTestType, testSetup.TestType);
    }
}
