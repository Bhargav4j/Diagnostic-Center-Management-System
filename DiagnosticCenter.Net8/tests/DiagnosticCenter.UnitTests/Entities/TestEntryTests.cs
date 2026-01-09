using Xunit;
using DiagnosticCenter.Domain.Entities;
using System;
using System.Collections.Generic;

namespace DiagnosticCenter.UnitTests.Entities;

public class TestEntryTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var testEntry = new TestEntry();

        // Assert
        Assert.Equal(0, testEntry.Id);
        Assert.Equal(string.Empty, testEntry.PatientName);
        Assert.Equal(string.Empty, testEntry.MobileNo);
        Assert.Equal(string.Empty, testEntry.BillNo);
        Assert.Equal(0, testEntry.TotalAmount);
        Assert.Equal(0, testEntry.PaidAmount);
        Assert.Equal(0, testEntry.TestId);
        Assert.False(testEntry.IsActive);
        Assert.Equal(string.Empty, testEntry.CreatedBy);
        Assert.Null(testEntry.ModifiedBy);
        Assert.Null(testEntry.Test);
        Assert.NotNull(testEntry.Payments);
        Assert.Empty(testEntry.Payments);
    }

    [Fact]
    public void Id_ShouldSetAndGetCorrectly()
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
    public void PatientName_ShouldSetAndGetCorrectly()
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
    public void DateOfBirth_ShouldSetAndGetCorrectly()
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
    public void MobileNo_ShouldSetAndGetCorrectly()
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
    public void BillNo_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var testEntry = new TestEntry();
        var expectedBillNo = "BILL-2024-001";

        // Act
        testEntry.BillNo = expectedBillNo;

        // Assert
        Assert.Equal(expectedBillNo, testEntry.BillNo);
    }

    [Fact]
    public void TotalAmount_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var testEntry = new TestEntry();
        var expectedAmount = 500.00m;

        // Act
        testEntry.TotalAmount = expectedAmount;

        // Assert
        Assert.Equal(expectedAmount, testEntry.TotalAmount);
    }

    [Fact]
    public void DueDate_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var testEntry = new TestEntry();
        var expectedDueDate = DateTime.Now.AddDays(7);

        // Act
        testEntry.DueDate = expectedDueDate;

        // Assert
        Assert.Equal(expectedDueDate, testEntry.DueDate);
    }

    [Fact]
    public void PaidAmount_ShouldSetAndGetCorrectly()
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
    public void TestId_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var testEntry = new TestEntry();
        var expectedTestId = 42;

        // Act
        testEntry.TestId = expectedTestId;

        // Assert
        Assert.Equal(expectedTestId, testEntry.TestId);
    }

    [Fact]
    public void DueAmount_ShouldCalculateCorrectly()
    {
        // Arrange
        var testEntry = new TestEntry
        {
            TotalAmount = 1000.00m,
            PaidAmount = 300.00m
        };

        // Act
        var dueAmount = testEntry.DueAmount;

        // Assert
        Assert.Equal(700.00m, dueAmount);
    }

    [Fact]
    public void DueAmount_ShouldBeZeroWhenFullyPaid()
    {
        // Arrange
        var testEntry = new TestEntry
        {
            TotalAmount = 500.00m,
            PaidAmount = 500.00m
        };

        // Act
        var dueAmount = testEntry.DueAmount;

        // Assert
        Assert.Equal(0.00m, dueAmount);
    }

    [Fact]
    public void DueAmount_ShouldEqualTotalWhenNothingPaid()
    {
        // Arrange
        var testEntry = new TestEntry
        {
            TotalAmount = 750.00m,
            PaidAmount = 0.00m
        };

        // Act
        var dueAmount = testEntry.DueAmount;

        // Assert
        Assert.Equal(750.00m, dueAmount);
    }

    [Fact]
    public void CreatedDate_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var testEntry = new TestEntry();
        var expectedDate = DateTime.Now;

        // Act
        testEntry.CreatedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, testEntry.CreatedDate);
    }

    [Fact]
    public void ModifiedDate_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var testEntry = new TestEntry();
        var expectedDate = DateTime.Now;

        // Act
        testEntry.ModifiedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, testEntry.ModifiedDate);
    }

    [Fact]
    public void ModifiedDate_ShouldAcceptNull()
    {
        // Arrange
        var testEntry = new TestEntry { ModifiedDate = DateTime.Now };

        // Act
        testEntry.ModifiedDate = null;

        // Assert
        Assert.Null(testEntry.ModifiedDate);
    }

    [Fact]
    public void IsActive_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var testEntry = new TestEntry();

        // Act
        testEntry.IsActive = true;

        // Assert
        Assert.True(testEntry.IsActive);
    }

    [Fact]
    public void CreatedBy_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var testEntry = new TestEntry();
        var expectedCreatedBy = "receptionist@clinic.com";

        // Act
        testEntry.CreatedBy = expectedCreatedBy;

        // Assert
        Assert.Equal(expectedCreatedBy, testEntry.CreatedBy);
    }

    [Fact]
    public void ModifiedBy_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var testEntry = new TestEntry();
        var expectedModifiedBy = "admin@clinic.com";

        // Act
        testEntry.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedModifiedBy, testEntry.ModifiedBy);
    }

    [Fact]
    public void Test_ShouldSetAndGetNavigationProperty()
    {
        // Arrange
        var testEntry = new TestEntry();
        var testSetup = new TestSetup { Id = 1, Name = "Blood Test", Fee = 100.00m };

        // Act
        testEntry.Test = testSetup;

        // Assert
        Assert.NotNull(testEntry.Test);
        Assert.Equal(testSetup.Id, testEntry.Test.Id);
        Assert.Equal(testSetup.Name, testEntry.Test.Name);
    }

    [Fact]
    public void Payments_ShouldSetAndGetCollection()
    {
        // Arrange
        var testEntry = new TestEntry();
        var payments = new List<Payment>
        {
            new Payment { Id = 1, Amount = 100.00m },
            new Payment { Id = 2, Amount = 200.00m }
        };

        // Act
        testEntry.Payments = payments;

        // Assert
        Assert.NotNull(testEntry.Payments);
        Assert.Equal(2, testEntry.Payments.Count);
    }

    [Fact]
    public void TestEntry_ShouldAllowCompleteObjectInitialization()
    {
        // Arrange
        var dob = new DateTime(1985, 3, 20);
        var createdDate = DateTime.Now;
        var dueDate = DateTime.Now.AddDays(14);

        // Act
        var testEntry = new TestEntry
        {
            Id = 999,
            PatientName = "Jane Smith",
            DateOfBirth = dob,
            MobileNo = "9876543210",
            BillNo = "BILL-2024-999",
            TotalAmount = 1500.00m,
            DueDate = dueDate,
            PaidAmount = 500.00m,
            TestId = 10,
            CreatedDate = createdDate,
            IsActive = true,
            CreatedBy = "staff@clinic.com",
            Test = new TestSetup { Id = 10, Name = "Complete Blood Count" }
        };

        // Assert
        Assert.Equal(999, testEntry.Id);
        Assert.Equal("Jane Smith", testEntry.PatientName);
        Assert.Equal(dob, testEntry.DateOfBirth);
        Assert.Equal("9876543210", testEntry.MobileNo);
        Assert.Equal("BILL-2024-999", testEntry.BillNo);
        Assert.Equal(1500.00m, testEntry.TotalAmount);
        Assert.Equal(500.00m, testEntry.PaidAmount);
        Assert.Equal(1000.00m, testEntry.DueAmount);
        Assert.Equal(10, testEntry.TestId);
        Assert.True(testEntry.IsActive);
        Assert.NotNull(testEntry.Test);
    }
}
