using Xunit;
using DiagnosticCenter.Domain.Entities;
using System;

namespace DiagnosticCenter.Domain.Entities.Tests;

public class PaymentTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange & Act
        var payment = new Payment();

        // Assert
        Assert.NotNull(payment);
        Assert.Equal(string.Empty, payment.BillNo);
        Assert.Equal(string.Empty, payment.CreatedBy);
    }

    [Fact]
    public void Id_SetAndGet_ShouldWork()
    {
        // Arrange
        var payment = new Payment();
        var expectedId = 111;

        // Act
        payment.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, payment.Id);
    }

    [Fact]
    public void TestEntryId_SetAndGet_ShouldWork()
    {
        // Arrange
        var payment = new Payment();
        var expectedTestEntryId = 5;

        // Act
        payment.TestEntryId = expectedTestEntryId;

        // Assert
        Assert.Equal(expectedTestEntryId, payment.TestEntryId);
    }

    [Fact]
    public void BillNo_SetAndGet_ShouldWork()
    {
        // Arrange
        var payment = new Payment();
        var expectedBillNo = "BILL-123";

        // Act
        payment.BillNo = expectedBillNo;

        // Assert
        Assert.Equal(expectedBillNo, payment.BillNo);
    }

    [Fact]
    public void Amount_SetAndGet_ShouldWork()
    {
        // Arrange
        var payment = new Payment();
        var expectedAmount = 500.25m;

        // Act
        payment.Amount = expectedAmount;

        // Assert
        Assert.Equal(expectedAmount, payment.Amount);
    }

    [Fact]
    public void Amount_SetZero_ShouldWork()
    {
        // Arrange
        var payment = new Payment();

        // Act
        payment.Amount = 0m;

        // Assert
        Assert.Equal(0m, payment.Amount);
    }

    [Fact]
    public void PaymentDate_SetAndGet_ShouldWork()
    {
        // Arrange
        var payment = new Payment();
        var expectedDate = DateTime.UtcNow;

        // Act
        payment.PaymentDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, payment.PaymentDate);
    }

    [Fact]
    public void PaymentMethod_SetAndGet_ShouldWork()
    {
        // Arrange
        var payment = new Payment();
        var expectedMethod = "Credit Card";

        // Act
        payment.PaymentMethod = expectedMethod;

        // Assert
        Assert.Equal(expectedMethod, payment.PaymentMethod);
    }

    [Fact]
    public void PaymentMethod_SetNull_ShouldWork()
    {
        // Arrange
        var payment = new Payment();

        // Act
        payment.PaymentMethod = null;

        // Assert
        Assert.Null(payment.PaymentMethod);
    }

    [Fact]
    public void TransactionId_SetAndGet_ShouldWork()
    {
        // Arrange
        var payment = new Payment();
        var expectedTransactionId = "TXN-456789";

        // Act
        payment.TransactionId = expectedTransactionId;

        // Assert
        Assert.Equal(expectedTransactionId, payment.TransactionId);
    }

    [Fact]
    public void TransactionId_SetNull_ShouldWork()
    {
        // Arrange
        var payment = new Payment();

        // Act
        payment.TransactionId = null;

        // Assert
        Assert.Null(payment.TransactionId);
    }

    [Fact]
    public void CreatedDate_SetAndGet_ShouldWork()
    {
        // Arrange
        var payment = new Payment();
        var expectedDate = DateTime.UtcNow;

        // Act
        payment.CreatedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, payment.CreatedDate);
    }

    [Fact]
    public void IsActive_SetAndGet_ShouldWork()
    {
        // Arrange
        var payment = new Payment();

        // Act
        payment.IsActive = true;

        // Assert
        Assert.True(payment.IsActive);
    }

    [Fact]
    public void TestEntry_NavigationProperty_CanBeSet()
    {
        // Arrange
        var payment = new Payment();
        var testEntry = new TestEntry { Id = 1, PatientName = "John Doe" };

        // Act
        payment.TestEntry = testEntry;

        // Assert
        Assert.NotNull(payment.TestEntry);
        Assert.Equal(testEntry, payment.TestEntry);
    }

    [Fact]
    public void AllProperties_SetAndGet_ShouldWork()
    {
        // Arrange
        var payment = new Payment
        {
            Id = 1,
            TestEntryId = 10,
            BillNo = "BILL-999",
            Amount = 750.00m,
            PaymentDate = DateTime.UtcNow,
            PaymentMethod = "Cash",
            TransactionId = "TXN-12345",
            CreatedDate = DateTime.UtcNow,
            IsActive = true,
            CreatedBy = "Accountant"
        };

        // Assert
        Assert.Equal(1, payment.Id);
        Assert.Equal(10, payment.TestEntryId);
        Assert.Equal("BILL-999", payment.BillNo);
        Assert.Equal(750.00m, payment.Amount);
        Assert.Equal("Cash", payment.PaymentMethod);
        Assert.Equal("TXN-12345", payment.TransactionId);
        Assert.True(payment.IsActive);
    }
}
