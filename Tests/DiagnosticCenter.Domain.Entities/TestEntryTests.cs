using Xunit;
using DiagnosticCenter.Domain.Entities;
using System;

namespace DiagnosticCenter.Domain.Entities.Tests;

public class TestEntryTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange & Act
        var testEntry = new TestEntry();

        // Assert
        Assert.NotNull(testEntry);
        Assert.Equal(string.Empty, testEntry.PatientName);
        Assert.Equal(string.Empty, testEntry.MobileNo);
        Assert.Equal(string.Empty, testEntry.BillNo);
        Assert.Equal(string.Empty, testEntry.CreatedBy);
        Assert.NotNull(testEntry.Payments);
    }

    [Fact]
    public void Id_SetAndGet_ShouldWork()
    {
        // Arrange
        var testEntry = new TestEntry();
        var expectedId = 789;

        // Act
        testEntry.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, testEntry.Id);
    }

    [Fact]
    public void PatientName_SetAndGet_ShouldWork()
    {
        // Arrange
        var testEntry = new TestEntry();
        var expectedName = "Jane Smith";

        // Act
        testEntry.PatientName = expectedName;

        // Assert
        Assert.Equal(expectedName, testEntry.PatientName);
    }

    [Fact]
    public void DateOfBirth_SetAndGet_ShouldWork()
    {
        // Arrange
        var testEntry = new TestEntry();
        var expectedDob = new DateTime(1990, 5, 15);

        // Act
        testEntry.DateOfBirth = expectedDob;

        // Assert
        Assert.Equal(expectedDob, testEntry.DateOfBirth);
    }

    [Fact]
    public void MobileNo_SetAndGet_ShouldWork()
    {
        // Arrange
        var testEntry = new TestEntry();
        var expectedMobile = "1234567890";

        // Act
        testEntry.MobileNo = expectedMobile;

        // Assert
        Assert.Equal(expectedMobile, testEntry.MobileNo);
    }

    [Fact]
    public void BillNo_SetAndGet_ShouldWork()
    {
        // Arrange
        var testEntry = new TestEntry();
        var expectedBillNo = "BILL-001";

        // Act
        testEntry.BillNo = expectedBillNo;

        // Assert
        Assert.Equal(expectedBillNo, testEntry.BillNo);
    }

    [Fact]
    public void TotalAmount_SetAndGet_ShouldWork()
    {
        // Arrange
        var testEntry = new TestEntry();
        var expectedAmount = 1500.75m;

        // Act
        testEntry.TotalAmount = expectedAmount;

        // Assert
        Assert.Equal(expectedAmount, testEntry.TotalAmount);
    }

    [Fact]
    public void TotalAmount_SetZero_ShouldWork()
    {
        // Arrange
        var testEntry = new TestEntry();

        // Act
        testEntry.TotalAmount = 0m;

        // Assert
        Assert.Equal(0m, testEntry.TotalAmount);
    }

    [Fact]
    public void DueDate_SetAndGet_ShouldWork()
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
    public void PaidAmount_SetAndGet_ShouldWork()
    {
        // Arrange
        var testEntry = new TestEntry();
        var expectedPaidAmount = 750.50m;

        // Act
        testEntry.PaidAmount = expectedPaidAmount;

        // Assert
        Assert.Equal(expectedPaidAmount, testEntry.PaidAmount);
    }

    [Fact]
    public void TestId_SetAndGet_ShouldWork()
    {
        // Arrange
        var testEntry = new TestEntry();
        var expectedTestId = 5;

        // Act
        testEntry.TestId = expectedTestId;

        // Assert
        Assert.Equal(expectedTestId, testEntry.TestId);
    }

    [Fact]
    public void IsActive_SetAndGet_ShouldWork()
    {
        // Arrange
        var testEntry = new TestEntry();

        // Act
        testEntry.IsActive = true;

        // Assert
        Assert.True(testEntry.IsActive);
    }

    [Fact]
    public void Test_NavigationProperty_CanBeSet()
    {
        // Arrange
        var testEntry = new TestEntry();
        var testSetup = new TestSetup { Id = 1, TestName = "Blood Test" };

        // Act
        testEntry.Test = testSetup;

        // Assert
        Assert.NotNull(testEntry.Test);
        Assert.Equal(testSetup, testEntry.Test);
    }

    [Fact]
    public void Payments_NavigationProperty_ShouldBeInitialized()
    {
        // Arrange
        var testEntry = new TestEntry();

        // Assert
        Assert.NotNull(testEntry.Payments);
        Assert.Empty(testEntry.Payments);
    }

    [Fact]
    public void Payments_CanAddItems_ShouldWork()
    {
        // Arrange
        var testEntry = new TestEntry();
        var payment = new Payment { Id = 1, Amount = 100m };

        // Act
        testEntry.Payments.Add(payment);

        // Assert
        Assert.Single(testEntry.Payments);
        Assert.Contains(payment, testEntry.Payments);
    }

    [Fact]
    public void AllProperties_SetAndGet_ShouldWork()
    {
        // Arrange
        var testEntry = new TestEntry
        {
            Id = 1,
            PatientName = "John Doe",
            DateOfBirth = new DateTime(1985, 3, 20),
            MobileNo = "9876543210",
            BillNo = "BILL-002",
            TotalAmount = 2000m,
            DueDate = DateTime.UtcNow.AddDays(10),
            PaidAmount = 1000m,
            TestId = 3,
            CreatedDate = DateTime.UtcNow,
            IsActive = true,
            CreatedBy = "Receptionist"
        };

        // Assert
        Assert.Equal(1, testEntry.Id);
        Assert.Equal("John Doe", testEntry.PatientName);
        Assert.Equal(new DateTime(1985, 3, 20), testEntry.DateOfBirth);
        Assert.Equal("9876543210", testEntry.MobileNo);
        Assert.Equal("BILL-002", testEntry.BillNo);
        Assert.Equal(2000m, testEntry.TotalAmount);
        Assert.Equal(1000m, testEntry.PaidAmount);
        Assert.Equal(3, testEntry.TestId);
        Assert.True(testEntry.IsActive);
    }
}
