using Xunit;
using DiagnosticCenter.Domain.Entities;

namespace Tests.DiagnosticCenter.Domain.Entities;

public class PaymentTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange & Act
        var payment = new Payment();

        // Assert
        Assert.Equal(0, payment.TestEntryId);
        Assert.Equal(string.Empty, payment.BillNumber);
        Assert.Equal(0, payment.Amount);
        Assert.Equal(default(DateTime), payment.PaymentDate);
        Assert.Null(payment.PaymentMethod);
        Assert.Null(payment.TransactionReference);
        Assert.Null(payment.Notes);
        Assert.Null(payment.TestEntry);
    }

    [Fact]
    public void TestEntryId_ShouldSetAndGetValue()
    {
        // Arrange
        var payment = new Payment();
        var expectedId = 123;

        // Act
        payment.TestEntryId = expectedId;

        // Assert
        Assert.Equal(expectedId, payment.TestEntryId);
    }

    [Fact]
    public void BillNumber_ShouldSetAndGetValue()
    {
        // Arrange
        var payment = new Payment();
        var expectedBillNumber = "BILL-2024010112345";

        // Act
        payment.BillNumber = expectedBillNumber;

        // Assert
        Assert.Equal(expectedBillNumber, payment.BillNumber);
    }

    [Fact]
    public void Amount_ShouldSetAndGetValue()
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
    public void Amount_ShouldHandleZeroValue()
    {
        // Arrange
        var payment = new Payment();

        // Act
        payment.Amount = 0m;

        // Assert
        Assert.Equal(0m, payment.Amount);
    }

    [Fact]
    public void PaymentDate_ShouldSetAndGetValue()
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
    public void PaymentMethod_ShouldSetAndGetValue()
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
    public void TransactionReference_ShouldSetAndGetValue()
    {
        // Arrange
        var payment = new Payment();
        var expectedReference = "TXN-123456789";

        // Act
        payment.TransactionReference = expectedReference;

        // Assert
        Assert.Equal(expectedReference, payment.TransactionReference);
    }

    [Fact]
    public void Notes_ShouldSetAndGetValue()
    {
        // Arrange
        var payment = new Payment();
        var expectedNotes = "Partial payment received";

        // Act
        payment.Notes = expectedNotes;

        // Assert
        Assert.Equal(expectedNotes, payment.Notes);
    }

    [Fact]
    public void TestEntry_ShouldSetAndGetValue()
    {
        // Arrange
        var payment = new Payment();
        var testEntry = new TestEntry { PatientName = "John Doe" };

        // Act
        payment.TestEntry = testEntry;

        // Assert
        Assert.Equal(testEntry, payment.TestEntry);
    }

    [Fact]
    public void InheritsFromBaseEntity_ShouldHaveBaseEntityProperties()
    {
        // Arrange & Act
        var payment = new Payment();

        // Assert
        Assert.IsAssignableFrom<BaseEntity>(payment);
        Assert.Equal(0, payment.Id);
    }
}
