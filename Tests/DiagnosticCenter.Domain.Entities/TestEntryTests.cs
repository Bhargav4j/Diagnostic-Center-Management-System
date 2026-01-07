using Xunit;
using DiagnosticCenter.Domain.Entities;

namespace Tests.DiagnosticCenter.Domain.Entities;

public class TestEntryTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange & Act
        var testEntry = new TestEntry();

        // Assert
        Assert.Equal(string.Empty, testEntry.PatientName);
        Assert.Equal(default(DateTime), testEntry.DateOfBirth);
        Assert.Equal(string.Empty, testEntry.MobileNumber);
        Assert.Equal(string.Empty, testEntry.BillNumber);
        Assert.Equal(0, testEntry.TotalAmount);
        Assert.Equal(default(DateTime), testEntry.DueDate);
        Assert.Equal(0, testEntry.PaidAmount);
        Assert.NotNull(testEntry.TestEntryItems);
        Assert.Empty(testEntry.TestEntryItems);
        Assert.NotNull(testEntry.Payments);
        Assert.Empty(testEntry.Payments);
    }

    [Fact]
    public void PatientName_ShouldSetAndGetValue()
    {
        // Arrange
        var testEntry = new TestEntry();
        var expectedName = "John Doe";

        // Act
        testEntry.PatientName = expectedName;

        // Assert
        Assert.Equal(expectedName, testEntry.PatientName);
    }

    [Fact]
    public void DateOfBirth_ShouldSetAndGetValue()
    {
        // Arrange
        var testEntry = new TestEntry();
        var expectedDate = new DateTime(1990, 5, 15);

        // Act
        testEntry.DateOfBirth = expectedDate;

        // Assert
        Assert.Equal(expectedDate, testEntry.DateOfBirth);
    }

    [Fact]
    public void MobileNumber_ShouldSetAndGetValue()
    {
        // Arrange
        var testEntry = new TestEntry();
        var expectedMobile = "1234567890";

        // Act
        testEntry.MobileNumber = expectedMobile;

        // Assert
        Assert.Equal(expectedMobile, testEntry.MobileNumber);
    }

    [Fact]
    public void BillNumber_ShouldSetAndGetValue()
    {
        // Arrange
        var testEntry = new TestEntry();
        var expectedBillNumber = "BILL-2024010112345";

        // Act
        testEntry.BillNumber = expectedBillNumber;

        // Assert
        Assert.Equal(expectedBillNumber, testEntry.BillNumber);
    }

    [Fact]
    public void TotalAmount_ShouldSetAndGetValue()
    {
        // Arrange
        var testEntry = new TestEntry();
        var expectedAmount = 500.50m;

        // Act
        testEntry.TotalAmount = expectedAmount;

        // Assert
        Assert.Equal(expectedAmount, testEntry.TotalAmount);
    }

    [Fact]
    public void DueDate_ShouldSetAndGetValue()
    {
        // Arrange
        var testEntry = new TestEntry();
        var expectedDueDate = DateTime.UtcNow.AddDays(7);

        // Act
        testEntry.DueDate = expectedDueDate;

        // Assert
        Assert.Equal(expectedDueDate, testEntry.DueDate);
    }

    [Fact]
    public void PaidAmount_ShouldSetAndGetValue()
    {
        // Arrange
        var testEntry = new TestEntry();
        var expectedPaidAmount = 200.00m;

        // Act
        testEntry.PaidAmount = expectedPaidAmount;

        // Assert
        Assert.Equal(expectedPaidAmount, testEntry.PaidAmount);
    }

    [Fact]
    public void DueAmount_ShouldCalculateCorrectly_WhenNoPayment()
    {
        // Arrange
        var testEntry = new TestEntry
        {
            TotalAmount = 500m,
            PaidAmount = 0m
        };

        // Act
        var dueAmount = testEntry.DueAmount;

        // Assert
        Assert.Equal(500m, dueAmount);
    }

    [Fact]
    public void DueAmount_ShouldCalculateCorrectly_WhenPartialPayment()
    {
        // Arrange
        var testEntry = new TestEntry
        {
            TotalAmount = 500m,
            PaidAmount = 200m
        };

        // Act
        var dueAmount = testEntry.DueAmount;

        // Assert
        Assert.Equal(300m, dueAmount);
    }

    [Fact]
    public void DueAmount_ShouldCalculateCorrectly_WhenFullyPaid()
    {
        // Arrange
        var testEntry = new TestEntry
        {
            TotalAmount = 500m,
            PaidAmount = 500m
        };

        // Act
        var dueAmount = testEntry.DueAmount;

        // Assert
        Assert.Equal(0m, dueAmount);
    }

    [Fact]
    public void TestEntryItems_ShouldBeInitializedAsEmptyList()
    {
        // Arrange & Act
        var testEntry = new TestEntry();

        // Assert
        Assert.NotNull(testEntry.TestEntryItems);
        Assert.IsAssignableFrom<ICollection<TestEntryItem>>(testEntry.TestEntryItems);
        Assert.Empty(testEntry.TestEntryItems);
    }

    [Fact]
    public void TestEntryItems_ShouldAllowAddingItems()
    {
        // Arrange
        var testEntry = new TestEntry();
        var item = new TestEntryItem { Fee = 100m };

        // Act
        testEntry.TestEntryItems.Add(item);

        // Assert
        Assert.Single(testEntry.TestEntryItems);
        Assert.Contains(item, testEntry.TestEntryItems);
    }

    [Fact]
    public void Payments_ShouldBeInitializedAsEmptyList()
    {
        // Arrange & Act
        var testEntry = new TestEntry();

        // Assert
        Assert.NotNull(testEntry.Payments);
        Assert.IsAssignableFrom<ICollection<Payment>>(testEntry.Payments);
        Assert.Empty(testEntry.Payments);
    }

    [Fact]
    public void Payments_ShouldAllowAddingPayments()
    {
        // Arrange
        var testEntry = new TestEntry();
        var payment = new Payment { Amount = 100m };

        // Act
        testEntry.Payments.Add(payment);

        // Assert
        Assert.Single(testEntry.Payments);
        Assert.Contains(payment, testEntry.Payments);
    }

    [Fact]
    public void InheritsFromBaseEntity_ShouldHaveBaseEntityProperties()
    {
        // Arrange & Act
        var testEntry = new TestEntry();

        // Assert
        Assert.IsAssignableFrom<BaseEntity>(testEntry);
        Assert.Equal(0, testEntry.Id);
    }
}
