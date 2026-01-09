using DiagnosticCenter.Domain.Entities;
using Xunit;
using System;

namespace DiagnosticCenter.UnitTests.Domain.Entities;

public class PatientTestTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties_WithDefaultValues()
    {
        // Arrange & Act
        var patientTest = new PatientTest();

        // Assert
        Assert.Equal(0, patientTest.Id);
        Assert.Equal(0, patientTest.PatientId);
        Assert.Equal(0, patientTest.TestSetupId);
        Assert.Equal(string.Empty, patientTest.BillNo);
        Assert.Equal(0m, patientTest.TotalAmount);
        Assert.Equal(0m, patientTest.PaidAmount);
        Assert.Equal(default(DateTime), patientTest.DueDate);
        Assert.Equal(default(DateTime), patientTest.CreatedDate);
        Assert.Null(patientTest.ModifiedDate);
        Assert.True(patientTest.IsActive);
        Assert.Equal(string.Empty, patientTest.CreatedBy);
        Assert.Null(patientTest.ModifiedBy);
        Assert.Null(patientTest.Patient);
        Assert.Null(patientTest.TestSetup);
        Assert.Null(patientTest.Payment);
    }

    [Fact]
    public void Id_ShouldSetAndGet_Correctly()
    {
        // Arrange
        var patientTest = new PatientTest();
        var expectedId = 555;

        // Act
        patientTest.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, patientTest.Id);
    }

    [Fact]
    public void PatientId_ShouldSetAndGet_Correctly()
    {
        // Arrange
        var patientTest = new PatientTest();
        var expectedPatientId = 101;

        // Act
        patientTest.PatientId = expectedPatientId;

        // Assert
        Assert.Equal(expectedPatientId, patientTest.PatientId);
    }

    [Fact]
    public void TestSetupId_ShouldSetAndGet_Correctly()
    {
        // Arrange
        var patientTest = new PatientTest();
        var expectedTestSetupId = 202;

        // Act
        patientTest.TestSetupId = expectedTestSetupId;

        // Assert
        Assert.Equal(expectedTestSetupId, patientTest.TestSetupId);
    }

    [Fact]
    public void BillNo_ShouldSetAndGet_Correctly()
    {
        // Arrange
        var patientTest = new PatientTest();
        var expectedBillNo = "BILL-2024-001";

        // Act
        patientTest.BillNo = expectedBillNo;

        // Assert
        Assert.Equal(expectedBillNo, patientTest.BillNo);
    }

    [Fact]
    public void BillNo_ShouldAcceptEmptyString()
    {
        // Arrange
        var patientTest = new PatientTest();

        // Act
        patientTest.BillNo = string.Empty;

        // Assert
        Assert.Equal(string.Empty, patientTest.BillNo);
    }

    [Fact]
    public void TotalAmount_ShouldSetAndGet_Correctly()
    {
        // Arrange
        var patientTest = new PatientTest();
        var expectedAmount = 1500.75m;

        // Act
        patientTest.TotalAmount = expectedAmount;

        // Assert
        Assert.Equal(expectedAmount, patientTest.TotalAmount);
    }

    [Fact]
    public void TotalAmount_ShouldAcceptZeroValue()
    {
        // Arrange
        var patientTest = new PatientTest();

        // Act
        patientTest.TotalAmount = 0m;

        // Assert
        Assert.Equal(0m, patientTest.TotalAmount);
    }

    [Fact]
    public void TotalAmount_ShouldAcceptNegativeValue()
    {
        // Arrange
        var patientTest = new PatientTest();

        // Act
        patientTest.TotalAmount = -100m;

        // Assert
        Assert.Equal(-100m, patientTest.TotalAmount);
    }

    [Fact]
    public void PaidAmount_ShouldSetAndGet_Correctly()
    {
        // Arrange
        var patientTest = new PatientTest();
        var expectedAmount = 500.25m;

        // Act
        patientTest.PaidAmount = expectedAmount;

        // Assert
        Assert.Equal(expectedAmount, patientTest.PaidAmount);
    }

    [Fact]
    public void PaidAmount_ShouldAcceptZeroValue()
    {
        // Arrange
        var patientTest = new PatientTest();

        // Act
        patientTest.PaidAmount = 0m;

        // Assert
        Assert.Equal(0m, patientTest.PaidAmount);
    }

    [Fact]
    public void DueDate_ShouldSetAndGet_Correctly()
    {
        // Arrange
        var patientTest = new PatientTest();
        var expectedDueDate = new DateTime(2024, 12, 31);

        // Act
        patientTest.DueDate = expectedDueDate;

        // Assert
        Assert.Equal(expectedDueDate, patientTest.DueDate);
    }

    [Fact]
    public void CreatedDate_ShouldSetAndGet_Correctly()
    {
        // Arrange
        var patientTest = new PatientTest();
        var expectedDate = DateTime.Now;

        // Act
        patientTest.CreatedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, patientTest.CreatedDate);
    }

    [Fact]
    public void ModifiedDate_ShouldAcceptNullValue()
    {
        // Arrange
        var patientTest = new PatientTest();

        // Act
        patientTest.ModifiedDate = null;

        // Assert
        Assert.Null(patientTest.ModifiedDate);
    }

    [Fact]
    public void ModifiedDate_ShouldSetAndGet_Correctly()
    {
        // Arrange
        var patientTest = new PatientTest();
        var expectedDate = DateTime.Now;

        // Act
        patientTest.ModifiedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, patientTest.ModifiedDate);
    }

    [Fact]
    public void IsActive_ShouldDefaultToTrue()
    {
        // Arrange & Act
        var patientTest = new PatientTest();

        // Assert
        Assert.True(patientTest.IsActive);
    }

    [Fact]
    public void IsActive_ShouldSetToFalse()
    {
        // Arrange
        var patientTest = new PatientTest();

        // Act
        patientTest.IsActive = false;

        // Assert
        Assert.False(patientTest.IsActive);
    }

    [Fact]
    public void CreatedBy_ShouldSetAndGet_Correctly()
    {
        // Arrange
        var patientTest = new PatientTest();
        var expectedCreatedBy = "Doctor1";

        // Act
        patientTest.CreatedBy = expectedCreatedBy;

        // Assert
        Assert.Equal(expectedCreatedBy, patientTest.CreatedBy);
    }

    [Fact]
    public void ModifiedBy_ShouldAcceptNullValue()
    {
        // Arrange
        var patientTest = new PatientTest();

        // Act
        patientTest.ModifiedBy = null;

        // Assert
        Assert.Null(patientTest.ModifiedBy);
    }

    [Fact]
    public void ModifiedBy_ShouldSetAndGet_Correctly()
    {
        // Arrange
        var patientTest = new PatientTest();
        var expectedModifiedBy = "Doctor2";

        // Act
        patientTest.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedModifiedBy, patientTest.ModifiedBy);
    }

    [Fact]
    public void Patient_ShouldAcceptNullValue()
    {
        // Arrange
        var patientTest = new PatientTest();

        // Act
        patientTest.Patient = null;

        // Assert
        Assert.Null(patientTest.Patient);
    }

    [Fact]
    public void Patient_ShouldSetAndGet_Correctly()
    {
        // Arrange
        var patientTest = new PatientTest();
        var patient = new Patient { Id = 1, Name = "John Doe" };

        // Act
        patientTest.Patient = patient;

        // Assert
        Assert.NotNull(patientTest.Patient);
        Assert.Equal(patient, patientTest.Patient);
        Assert.Equal(1, patientTest.Patient.Id);
        Assert.Equal("John Doe", patientTest.Patient.Name);
    }

    [Fact]
    public void TestSetup_ShouldAcceptNullValue()
    {
        // Arrange
        var patientTest = new PatientTest();

        // Act
        patientTest.TestSetup = null;

        // Assert
        Assert.Null(patientTest.TestSetup);
    }

    [Fact]
    public void TestSetup_ShouldSetAndGet_Correctly()
    {
        // Arrange
        var patientTest = new PatientTest();
        var testSetup = new TestSetup { Id = 1, Name = "Blood Test" };

        // Act
        patientTest.TestSetup = testSetup;

        // Assert
        Assert.NotNull(patientTest.TestSetup);
        Assert.Equal(testSetup, patientTest.TestSetup);
        Assert.Equal(1, patientTest.TestSetup.Id);
        Assert.Equal("Blood Test", patientTest.TestSetup.Name);
    }

    [Fact]
    public void Payment_ShouldAcceptNullValue()
    {
        // Arrange
        var patientTest = new PatientTest();

        // Act
        patientTest.Payment = null;

        // Assert
        Assert.Null(patientTest.Payment);
    }

    [Fact]
    public void Payment_ShouldSetAndGet_Correctly()
    {
        // Arrange
        var patientTest = new PatientTest();
        var payment = new Payment { Id = 1, AmountPaid = 500m };

        // Act
        patientTest.Payment = payment;

        // Assert
        Assert.NotNull(patientTest.Payment);
        Assert.Equal(payment, patientTest.Payment);
        Assert.Equal(1, patientTest.Payment.Id);
        Assert.Equal(500m, patientTest.Payment.AmountPaid);
    }

    [Fact]
    public void AllProperties_ShouldSetAndGet_Correctly()
    {
        // Arrange
        var patientTest = new PatientTest();
        var expectedId = 777;
        var expectedPatientId = 888;
        var expectedTestSetupId = 999;
        var expectedBillNo = "BILL-2024-999";
        var expectedTotalAmount = 2500.50m;
        var expectedPaidAmount = 1500.25m;
        var expectedDueDate = new DateTime(2024, 12, 15);
        var expectedCreatedDate = new DateTime(2024, 1, 10);
        var expectedModifiedDate = new DateTime(2024, 2, 20);
        var expectedIsActive = false;
        var expectedCreatedBy = "Admin1";
        var expectedModifiedBy = "Admin2";

        // Act
        patientTest.Id = expectedId;
        patientTest.PatientId = expectedPatientId;
        patientTest.TestSetupId = expectedTestSetupId;
        patientTest.BillNo = expectedBillNo;
        patientTest.TotalAmount = expectedTotalAmount;
        patientTest.PaidAmount = expectedPaidAmount;
        patientTest.DueDate = expectedDueDate;
        patientTest.CreatedDate = expectedCreatedDate;
        patientTest.ModifiedDate = expectedModifiedDate;
        patientTest.IsActive = expectedIsActive;
        patientTest.CreatedBy = expectedCreatedBy;
        patientTest.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedId, patientTest.Id);
        Assert.Equal(expectedPatientId, patientTest.PatientId);
        Assert.Equal(expectedTestSetupId, patientTest.TestSetupId);
        Assert.Equal(expectedBillNo, patientTest.BillNo);
        Assert.Equal(expectedTotalAmount, patientTest.TotalAmount);
        Assert.Equal(expectedPaidAmount, patientTest.PaidAmount);
        Assert.Equal(expectedDueDate, patientTest.DueDate);
        Assert.Equal(expectedCreatedDate, patientTest.CreatedDate);
        Assert.Equal(expectedModifiedDate, patientTest.ModifiedDate);
        Assert.Equal(expectedIsActive, patientTest.IsActive);
        Assert.Equal(expectedCreatedBy, patientTest.CreatedBy);
        Assert.Equal(expectedModifiedBy, patientTest.ModifiedBy);
    }
}
