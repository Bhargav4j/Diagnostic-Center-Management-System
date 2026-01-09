using Xunit;
using DiagnosticCenter.Domain.Entities;
using System;

namespace DiagnosticCenter.UnitTests.Entities;

public class PaymentTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var payment = new Payment();

        // Assert
        Assert.Equal(0, payment.Id);
        Assert.Equal(string.Empty, payment.BillNo);
        Assert.Equal(0, payment.TestEntryId);
        Assert.Equal(0, payment.Amount);
        Assert.Equal(string.Empty, payment.PaymentMethod);
        Assert.Null(payment.Remarks);
        Assert.False(payment.IsActive);
        Assert.Equal(string.Empty, payment.CreatedBy);
        Assert.Null(payment.ModifiedBy);
        Assert.Null(payment.TestEntry);
    }

    [Fact]
    public void Id_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var payment = new Payment();
        var expectedId = 101;

        // Act
        payment.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, payment.Id);
    }

    [Fact]
    public void BillNo_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var payment = new Payment();
        var expectedBillNo = "BILL-2024-001";

        // Act
        payment.BillNo = expectedBillNo;

        // Assert
        Assert.Equal(expectedBillNo, payment.BillNo);
    }

    [Fact]
    public void TestEntryId_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var payment = new Payment();
        var expectedTestEntryId = 555;

        // Act
        payment.TestEntryId = expectedTestEntryId;

        // Assert
        Assert.Equal(expectedTestEntryId, payment.TestEntryId);
    }

    [Fact]
    public void Amount_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var payment = new Payment();
        var expectedAmount = 250.75m;

        // Act
        payment.Amount = expectedAmount;

        // Assert
        Assert.Equal(expectedAmount, payment.Amount);
    }

    [Fact]
    public void PaymentDate_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var payment = new Payment();
        var expectedDate = DateTime.Now;

        // Act
        payment.PaymentDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, payment.PaymentDate);
    }

    [Fact]
    public void PaymentMethod_ShouldSetAndGetCorrectly()
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
    public void Remarks_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var payment = new Payment();
        var expectedRemarks = "Partial payment received";

        // Act
        payment.Remarks = expectedRemarks;

        // Assert
        Assert.Equal(expectedRemarks, payment.Remarks);
    }

    [Fact]
    public void Remarks_ShouldAcceptNull()
    {
        // Arrange
        var payment = new Payment { Remarks = "Some remarks" };

        // Act
        payment.Remarks = null;

        // Assert
        Assert.Null(payment.Remarks);
    }

    [Fact]
    public void CreatedDate_ShouldSetAndGetCorrectly()
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
    public void ModifiedDate_ShouldSetAndGetCorrectly()
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
    public void ModifiedDate_ShouldAcceptNull()
    {
        // Arrange
        var payment = new Payment { ModifiedDate = DateTime.Now };

        // Act
        payment.ModifiedDate = null;

        // Assert
        Assert.Null(payment.ModifiedDate);
    }

    [Fact]
    public void IsActive_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var payment = new Payment();

        // Act
        payment.IsActive = true;

        // Assert
        Assert.True(payment.IsActive);
    }

    [Fact]
    public void CreatedBy_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var payment = new Payment();
        var expectedCreatedBy = "cashier@clinic.com";

        // Act
        payment.CreatedBy = expectedCreatedBy;

        // Assert
        Assert.Equal(expectedCreatedBy, payment.CreatedBy);
    }

    [Fact]
    public void ModifiedBy_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var payment = new Payment();
        var expectedModifiedBy = "manager@clinic.com";

        // Act
        payment.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedModifiedBy, payment.ModifiedBy);
    }

    [Fact]
    public void ModifiedBy_ShouldAcceptNull()
    {
        // Arrange
        var payment = new Payment { ModifiedBy = "user@test.com" };

        // Act
        payment.ModifiedBy = null;

        // Assert
        Assert.Null(payment.ModifiedBy);
    }

    [Fact]
    public void TestEntry_ShouldSetAndGetNavigationProperty()
    {
        // Arrange
        var payment = new Payment();
        var testEntry = new TestEntry
        {
            Id = 1,
            PatientName = "John Doe",
            BillNo = "BILL-001"
        };

        // Act
        payment.TestEntry = testEntry;

        // Assert
        Assert.NotNull(payment.TestEntry);
        Assert.Equal(testEntry.Id, payment.TestEntry.Id);
        Assert.Equal(testEntry.PatientName, payment.TestEntry.PatientName);
    }

    [Fact]
    public void Payment_ShouldAllowCompleteObjectInitialization()
    {
        // Arrange
        var paymentDate = DateTime.Now;
        var createdDate = DateTime.Now;

        // Act
        var payment = new Payment
        {
            Id = 300,
            BillNo = "BILL-2024-300",
            TestEntryId = 50,
            Amount = 500.00m,
            PaymentDate = paymentDate,
            PaymentMethod = "Cash",
            Remarks = "Full payment received",
            CreatedDate = createdDate,
            IsActive = true,
            CreatedBy = "cashier",
            TestEntry = new TestEntry
            {
                Id = 50,
                PatientName = "Jane Smith"
            }
        };

        // Assert
        Assert.Equal(300, payment.Id);
        Assert.Equal("BILL-2024-300", payment.BillNo);
        Assert.Equal(50, payment.TestEntryId);
        Assert.Equal(500.00m, payment.Amount);
        Assert.Equal(paymentDate, payment.PaymentDate);
        Assert.Equal("Cash", payment.PaymentMethod);
        Assert.Equal("Full payment received", payment.Remarks);
        Assert.True(payment.IsActive);
        Assert.Equal("cashier", payment.CreatedBy);
        Assert.NotNull(payment.TestEntry);
    }

    [Theory]
    [InlineData("Cash")]
    [InlineData("Credit Card")]
    [InlineData("Debit Card")]
    [InlineData("Bank Transfer")]
    public void PaymentMethod_ShouldAcceptVariousPaymentTypes(string method)
    {
        // Arrange
        var payment = new Payment();

        // Act
        payment.PaymentMethod = method;

        // Assert
        Assert.Equal(method, payment.PaymentMethod);
    }

    [Theory]
    [InlineData(0.00)]
    [InlineData(50.50)]
    [InlineData(1000.00)]
    [InlineData(9999.99)]
    public void Amount_ShouldAcceptVariousAmounts(decimal amount)
    {
        // Arrange
        var payment = new Payment();

        // Act
        payment.Amount = amount;

        // Assert
        Assert.Equal(amount, payment.Amount);
    }
}
