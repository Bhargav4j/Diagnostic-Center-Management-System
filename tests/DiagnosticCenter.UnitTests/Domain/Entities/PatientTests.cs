using DiagnosticCenter.Domain.Entities;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiagnosticCenter.UnitTests.Domain.Entities;

public class PatientTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties_WithDefaultValues()
    {
        // Arrange & Act
        var patient = new Patient();

        // Assert
        Assert.Equal(0, patient.Id);
        Assert.Equal(string.Empty, patient.Name);
        Assert.Equal(default(DateTime), patient.DateOfBirth);
        Assert.Equal(string.Empty, patient.MobileNo);
        Assert.Equal(default(DateTime), patient.CreatedDate);
        Assert.Null(patient.ModifiedDate);
        Assert.True(patient.IsActive);
        Assert.Equal(string.Empty, patient.CreatedBy);
        Assert.Null(patient.ModifiedBy);
        Assert.NotNull(patient.PatientTests);
        Assert.Empty(patient.PatientTests);
    }

    [Fact]
    public void Id_ShouldSetAndGet_Correctly()
    {
        // Arrange
        var patient = new Patient();
        var expectedId = 100;

        // Act
        patient.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, patient.Id);
    }

    [Fact]
    public void Name_ShouldSetAndGet_Correctly()
    {
        // Arrange
        var patient = new Patient();
        var expectedName = "John Doe";

        // Act
        patient.Name = expectedName;

        // Assert
        Assert.Equal(expectedName, patient.Name);
    }

    [Fact]
    public void Name_ShouldAcceptEmptyString()
    {
        // Arrange
        var patient = new Patient();

        // Act
        patient.Name = string.Empty;

        // Assert
        Assert.Equal(string.Empty, patient.Name);
    }

    [Fact]
    public void DateOfBirth_ShouldSetAndGet_Correctly()
    {
        // Arrange
        var patient = new Patient();
        var expectedDob = new DateTime(1990, 5, 15);

        // Act
        patient.DateOfBirth = expectedDob;

        // Assert
        Assert.Equal(expectedDob, patient.DateOfBirth);
    }

    [Fact]
    public void DateOfBirth_ShouldAcceptMinValue()
    {
        // Arrange
        var patient = new Patient();

        // Act
        patient.DateOfBirth = DateTime.MinValue;

        // Assert
        Assert.Equal(DateTime.MinValue, patient.DateOfBirth);
    }

    [Fact]
    public void DateOfBirth_ShouldAcceptMaxValue()
    {
        // Arrange
        var patient = new Patient();

        // Act
        patient.DateOfBirth = DateTime.MaxValue;

        // Assert
        Assert.Equal(DateTime.MaxValue, patient.DateOfBirth);
    }

    [Fact]
    public void MobileNo_ShouldSetAndGet_Correctly()
    {
        // Arrange
        var patient = new Patient();
        var expectedMobileNo = "1234567890";

        // Act
        patient.MobileNo = expectedMobileNo;

        // Assert
        Assert.Equal(expectedMobileNo, patient.MobileNo);
    }

    [Fact]
    public void MobileNo_ShouldAcceptEmptyString()
    {
        // Arrange
        var patient = new Patient();

        // Act
        patient.MobileNo = string.Empty;

        // Assert
        Assert.Equal(string.Empty, patient.MobileNo);
    }

    [Fact]
    public void MobileNo_ShouldAcceptSpecialCharacters()
    {
        // Arrange
        var patient = new Patient();
        var mobileWithChars = "+1-234-567-8900";

        // Act
        patient.MobileNo = mobileWithChars;

        // Assert
        Assert.Equal(mobileWithChars, patient.MobileNo);
    }

    [Fact]
    public void CreatedDate_ShouldSetAndGet_Correctly()
    {
        // Arrange
        var patient = new Patient();
        var expectedDate = DateTime.Now;

        // Act
        patient.CreatedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, patient.CreatedDate);
    }

    [Fact]
    public void ModifiedDate_ShouldAcceptNullValue()
    {
        // Arrange
        var patient = new Patient();

        // Act
        patient.ModifiedDate = null;

        // Assert
        Assert.Null(patient.ModifiedDate);
    }

    [Fact]
    public void ModifiedDate_ShouldSetAndGet_Correctly()
    {
        // Arrange
        var patient = new Patient();
        var expectedDate = DateTime.Now;

        // Act
        patient.ModifiedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, patient.ModifiedDate);
    }

    [Fact]
    public void IsActive_ShouldDefaultToTrue()
    {
        // Arrange & Act
        var patient = new Patient();

        // Assert
        Assert.True(patient.IsActive);
    }

    [Fact]
    public void IsActive_ShouldSetToFalse()
    {
        // Arrange
        var patient = new Patient();

        // Act
        patient.IsActive = false;

        // Assert
        Assert.False(patient.IsActive);
    }

    [Fact]
    public void CreatedBy_ShouldSetAndGet_Correctly()
    {
        // Arrange
        var patient = new Patient();
        var expectedCreatedBy = "Receptionist1";

        // Act
        patient.CreatedBy = expectedCreatedBy;

        // Assert
        Assert.Equal(expectedCreatedBy, patient.CreatedBy);
    }

    [Fact]
    public void ModifiedBy_ShouldAcceptNullValue()
    {
        // Arrange
        var patient = new Patient();

        // Act
        patient.ModifiedBy = null;

        // Assert
        Assert.Null(patient.ModifiedBy);
    }

    [Fact]
    public void ModifiedBy_ShouldSetAndGet_Correctly()
    {
        // Arrange
        var patient = new Patient();
        var expectedModifiedBy = "Receptionist2";

        // Act
        patient.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedModifiedBy, patient.ModifiedBy);
    }

    [Fact]
    public void PatientTests_ShouldInitializeAsEmptyCollection()
    {
        // Arrange & Act
        var patient = new Patient();

        // Assert
        Assert.NotNull(patient.PatientTests);
        Assert.Empty(patient.PatientTests);
    }

    [Fact]
    public void PatientTests_ShouldAllowAddingItems()
    {
        // Arrange
        var patient = new Patient();
        var patientTest = new PatientTest { Id = 1 };

        // Act
        patient.PatientTests.Add(patientTest);

        // Assert
        Assert.Single(patient.PatientTests);
        Assert.Contains(patientTest, patient.PatientTests);
    }

    [Fact]
    public void PatientTests_ShouldAllowMultipleItems()
    {
        // Arrange
        var patient = new Patient();
        var patientTest1 = new PatientTest { Id = 1 };
        var patientTest2 = new PatientTest { Id = 2 };

        // Act
        patient.PatientTests.Add(patientTest1);
        patient.PatientTests.Add(patientTest2);

        // Assert
        Assert.Equal(2, patient.PatientTests.Count);
        Assert.Contains(patientTest1, patient.PatientTests);
        Assert.Contains(patientTest2, patient.PatientTests);
    }

    [Fact]
    public void AllProperties_ShouldSetAndGet_Correctly()
    {
        // Arrange
        var patient = new Patient();
        var expectedId = 999;
        var expectedName = "Jane Smith";
        var expectedDob = new DateTime(1985, 10, 20);
        var expectedMobileNo = "9876543210";
        var expectedCreatedDate = new DateTime(2024, 2, 1);
        var expectedModifiedDate = new DateTime(2024, 3, 15);
        var expectedIsActive = false;
        var expectedCreatedBy = "Admin1";
        var expectedModifiedBy = "Admin2";

        // Act
        patient.Id = expectedId;
        patient.Name = expectedName;
        patient.DateOfBirth = expectedDob;
        patient.MobileNo = expectedMobileNo;
        patient.CreatedDate = expectedCreatedDate;
        patient.ModifiedDate = expectedModifiedDate;
        patient.IsActive = expectedIsActive;
        patient.CreatedBy = expectedCreatedBy;
        patient.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedId, patient.Id);
        Assert.Equal(expectedName, patient.Name);
        Assert.Equal(expectedDob, patient.DateOfBirth);
        Assert.Equal(expectedMobileNo, patient.MobileNo);
        Assert.Equal(expectedCreatedDate, patient.CreatedDate);
        Assert.Equal(expectedModifiedDate, patient.ModifiedDate);
        Assert.Equal(expectedIsActive, patient.IsActive);
        Assert.Equal(expectedCreatedBy, patient.CreatedBy);
        Assert.Equal(expectedModifiedBy, patient.ModifiedBy);
    }
}
