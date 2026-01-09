using DiagnosticCenter.Domain.Entities;
using Xunit;
using System;

namespace DiagnosticCenter.UnitTests.Domain.Entities;

public class PaymentTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties_WithDefaultValues()
    {
        // Arrange & Act
        var payment = new Payment();

        // Assert
        Assert.Equal(0, payment.Id);
        Assert.Equal(0, payment.PatientTestId);
        Assert.Equal(string.Empty, payment.BillNo);
        Assert.Equal(0m, payment.AmountPaid);
        Assert.Equal(default(DateTime), payment.PaymentDate);
        Assert.Equal(default(DateTime), payment.CreatedDate);
        Assert.Null(payment.ModifiedDate);
        Assert.True(payment.IsActive);
        Assert.Equal(string.Empty, payment.CreatedBy);
        Assert.Null(payment.ModifiedBy);
        Assert.Null(payment.PatientTest);
    }

    [Fact]
    public void Id_ShouldSetAndGet_Correctly()
    {
        // Arrange
        var payment = new Payment();
        var expectedId = 333;

        // Act
        payment.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, payment.Id);
    }

    [Fact]
    public void PatientTestId_ShouldSetAndGet_Correctly()
    {
        // Arrange
        var payment = new Payment();
        var expectedPatientTestId = 444;

        // Act
        payment.PatientTestId = expectedPatientTestId;

        // Assert
        Assert.Equal(expectedPatientTestId, payment.PatientTestId);
    }

    [Fact]
    public void BillNo_ShouldSetAndGet_Correctly()
    {
        // Arrange
        var payment = new Payment();
        var expectedBillNo = "BILL-2024-555";

        // Act
        payment.BillNo = expectedBillNo;

        // Assert
        Assert.Equal(expectedBillNo, payment.BillNo);
    }

    [Fact]
    public void BillNo_ShouldAcceptEmptyString()
    {
        // Arrange
        var payment = new Payment();

        // Act
        payment.BillNo = string.Empty;

        // Assert
        Assert.Equal(string.Empty, payment.BillNo);
    }

    [Fact]
    public void AmountPaid_ShouldSetAndGet_Correctly()
    {
        // Arrange
        var payment = new Payment();
        var expectedAmount = 750.50m;

        // Act
        payment.AmountPaid = expectedAmount;

        // Assert
        Assert.Equal(expectedAmount, payment.AmountPaid);
    }

    [Fact]
    public void AmountPaid_ShouldAcceptZeroValue()
    {
        // Arrange
        var payment = new Payment();

        // Act
        payment.AmountPaid = 0m;

        // Assert
        Assert.Equal(0m, payment.AmountPaid);
    }

    [Fact]
    public void AmountPaid_ShouldAcceptNegativeValue()
    {
        // Arrange
        var payment = new Payment();

        // Act
        payment.AmountPaid = -50m;

        // Assert
        Assert.Equal(-50m, payment.AmountPaid);
    }

    [Fact]
    public void AmountPaid_ShouldAcceptLargeValue()
    {
        // Arrange
        var payment = new Payment();
        var largeAmount = 999999.99m;

        // Act
        payment.AmountPaid = largeAmount;

        // Assert
        Assert.Equal(largeAmount, payment.AmountPaid);
    }

    [Fact]
    public void PaymentDate_ShouldSetAndGet_Correctly()
    {
        // Arrange
        var payment = new Payment();
        var expectedPaymentDate = new DateTime(2024, 5, 15);

        // Act
        payment.PaymentDate = expectedPaymentDate;

        // Assert
        Assert.Equal(expectedPaymentDate, payment.PaymentDate);
    }

    [Fact]
    public void PaymentDate_ShouldAcceptMinValue()
    {
        // Arrange
        var payment = new Payment();

        // Act
        payment.PaymentDate = DateTime.MinValue;

        // Assert
        Assert.Equal(DateTime.MinValue, payment.PaymentDate);
    }

    [Fact]
    public void PaymentDate_ShouldAcceptMaxValue()
    {
        // Arrange
        var payment = new Payment();

        // Act
        payment.PaymentDate = DateTime.MaxValue;

        // Assert
        Assert.Equal(DateTime.MaxValue, payment.PaymentDate);
    }

    [Fact]
    public void CreatedDate_ShouldSetAndGet_Correctly()
    {
        // Arrange
        var payment = new Payment();
        var expectedDate = DateTime.Now;

        // Act
        payment.CreatedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, payment.CreatedDate);
    }

    [Fact]
    public void ModifiedDate_ShouldAcceptNullValue()
    {
        // Arrange
        var payment = new Payment();

        // Act
        payment.ModifiedDate = null;

        // Assert
        Assert.Null(payment.ModifiedDate);
    }

    [Fact]
    public void ModifiedDate_ShouldSetAndGet_Correctly()
    {
        // Arrange
        var payment = new Payment();
        var expectedDate = DateTime.Now;

        // Act
        payment.ModifiedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, payment.ModifiedDate);
    }

    [Fact]
    public void IsActive_ShouldDefaultToTrue()
    {
        // Arrange & Act
        var payment = new Payment();

        // Assert
        Assert.True(payment.IsActive);
    }

    [Fact]
    public void IsActive_ShouldSetToFalse()
    {
        // Arrange
        var payment = new Payment();

        // Act
        payment.IsActive = false;

        // Assert
        Assert.False(payment.IsActive);
    }

    [Fact]
    public void CreatedBy_ShouldSetAndGet_Correctly()
    {
        // Arrange
        var payment = new Payment();
        var expectedCreatedBy = "Cashier1";

        // Act
        payment.CreatedBy = expectedCreatedBy;

        // Assert
        Assert.Equal(expectedCreatedBy, payment.CreatedBy);
    }

    [Fact]
    public void ModifiedBy_ShouldAcceptNullValue()
    {
        // Arrange
        var payment = new Payment();

        // Act
        payment.ModifiedBy = null;

        // Assert
        Assert.Null(payment.ModifiedBy);
    }

    [Fact]
    public void ModifiedBy_ShouldSetAndGet_Correctly()
    {
        // Arrange
        var payment = new Payment();
        var expectedModifiedBy = "Cashier2";

        // Act
        payment.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedModifiedBy, payment.ModifiedBy);
    }

    [Fact]
    public void PatientTest_ShouldAcceptNullValue()
    {
        // Arrange
        var payment = new Payment();

        // Act
        payment.PatientTest = null;

        // Assert
        Assert.Null(payment.PatientTest);
    }

    [Fact]
    public void PatientTest_ShouldSetAndGet_Correctly()
    {
        // Arrange
        var payment = new Payment();
        var patientTest = new PatientTest { Id = 1, BillNo = "BILL-001" };

        // Act
        payment.PatientTest = patientTest;

        // Assert
        Assert.NotNull(payment.PatientTest);
        Assert.Equal(patientTest, payment.PatientTest);
        Assert.Equal(1, payment.PatientTest.Id);
        Assert.Equal("BILL-001", payment.PatientTest.BillNo);
    }

    [Fact]
    public void AllProperties_ShouldSetAndGet_Correctly()
    {
        // Arrange
        var payment = new Payment();
        var expectedId = 111;
        var expectedPatientTestId = 222;
        var expectedBillNo = "BILL-2024-333";
        var expectedAmountPaid = 1200.75m;
        var expectedPaymentDate = new DateTime(2024, 6, 10);
        var expectedCreatedDate = new DateTime(2024, 6, 11);
        var expectedModifiedDate = new DateTime(2024, 6, 12);
        var expectedIsActive = false;
        var expectedCreatedBy = "Finance1";
        var expectedModifiedBy = "Finance2";

        // Act
        payment.Id = expectedId;
        payment.PatientTestId = expectedPatientTestId;
        payment.BillNo = expectedBillNo;
        payment.AmountPaid = expectedAmountPaid;
        payment.PaymentDate = expectedPaymentDate;
        payment.CreatedDate = expectedCreatedDate;
        payment.ModifiedDate = expectedModifiedDate;
        payment.IsActive = expectedIsActive;
        payment.CreatedBy = expectedCreatedBy;
        payment.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedId, payment.Id);
        Assert.Equal(expectedPatientTestId, payment.PatientTestId);
        Assert.Equal(expectedBillNo, payment.BillNo);
        Assert.Equal(expectedAmountPaid, payment.AmountPaid);
        Assert.Equal(expectedPaymentDate, payment.PaymentDate);
        Assert.Equal(expectedCreatedDate, payment.CreatedDate);
        Assert.Equal(expectedModifiedDate, payment.ModifiedDate);
        Assert.Equal(expectedIsActive, payment.IsActive);
        Assert.Equal(expectedCreatedBy, payment.CreatedBy);
        Assert.Equal(expectedModifiedBy, payment.ModifiedBy);
    }
}
