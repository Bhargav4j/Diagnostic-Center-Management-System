using Xunit;
using DiagnosticCenter.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Tests.DiagnosticCenter.Domain;

public class TestEntryTests
{
    [Fact]
    public void TestEntry_Constructor_InitializesWithDefaultValues()
    {
        var testEntry = new TestEntry();

        Assert.Equal(0, testEntry.Id);
        Assert.Equal(string.Empty, testEntry.BillNo);
        Assert.Equal(string.Empty, testEntry.PatientName);
        Assert.Equal(0, testEntry.PatientAge);
        Assert.Equal(string.Empty, testEntry.PatientGender);
        Assert.Null(testEntry.ContactNumber);
        Assert.Equal(0, testEntry.TestSetupId);
        Assert.Equal(0, testEntry.TotalFee);
        Assert.False(testEntry.IsPaid);
        Assert.Equal("System", testEntry.CreatedBy);
        Assert.Null(testEntry.ModifiedBy);
        Assert.NotNull(testEntry.Payments);
        Assert.Empty(testEntry.Payments);
    }

    [Fact]
    public void TestEntry_SetBillNo_UpdatesBillNo()
    {
        var testEntry = new TestEntry { BillNo = "BILL001" };

        Assert.Equal("BILL001", testEntry.BillNo);
    }

    [Fact]
    public void TestEntry_SetPatientName_UpdatesPatientName()
    {
        var testEntry = new TestEntry { PatientName = "John Doe" };

        Assert.Equal("John Doe", testEntry.PatientName);
    }

    [Fact]
    public void TestEntry_SetPatientAge_UpdatesPatientAge()
    {
        var testEntry = new TestEntry { PatientAge = 35 };

        Assert.Equal(35, testEntry.PatientAge);
    }

    [Fact]
    public void TestEntry_SetPatientGender_UpdatesPatientGender()
    {
        var testEntry = new TestEntry { PatientGender = "Male" };

        Assert.Equal("Male", testEntry.PatientGender);
    }

    [Fact]
    public void TestEntry_SetContactNumber_UpdatesContactNumber()
    {
        var testEntry = new TestEntry { ContactNumber = "123-456-7890" };

        Assert.Equal("123-456-7890", testEntry.ContactNumber);
    }

    [Fact]
    public void TestEntry_SetTestSetupId_UpdatesTestSetupId()
    {
        var testEntry = new TestEntry { TestSetupId = 1 };

        Assert.Equal(1, testEntry.TestSetupId);
    }

    [Fact]
    public void TestEntry_SetTestDate_UpdatesTestDate()
    {
        var date = DateTime.UtcNow;
        var testEntry = new TestEntry { TestDate = date };

        Assert.Equal(date, testEntry.TestDate);
    }

    [Fact]
    public void TestEntry_SetTotalFee_UpdatesTotalFee()
    {
        var testEntry = new TestEntry { TotalFee = 500.00m };

        Assert.Equal(500.00m, testEntry.TotalFee);
    }

    [Fact]
    public void TestEntry_SetIsPaid_UpdatesIsPaid()
    {
        var testEntry = new TestEntry { IsPaid = true };

        Assert.True(testEntry.IsPaid);
    }

    [Fact]
    public void TestEntry_SetCreatedDate_UpdatesCreatedDate()
    {
        var date = DateTime.UtcNow.AddDays(-1);
        var testEntry = new TestEntry { CreatedDate = date };

        Assert.Equal(date, testEntry.CreatedDate);
    }

    [Fact]
    public void TestEntry_SetModifiedDate_UpdatesModifiedDate()
    {
        var date = DateTime.UtcNow;
        var testEntry = new TestEntry { ModifiedDate = date };

        Assert.Equal(date, testEntry.ModifiedDate);
    }

    [Fact]
    public void TestEntry_SetCreatedBy_UpdatesCreatedBy()
    {
        var testEntry = new TestEntry { CreatedBy = "Receptionist" };

        Assert.Equal("Receptionist", testEntry.CreatedBy);
    }

    [Fact]
    public void TestEntry_SetModifiedBy_UpdatesModifiedBy()
    {
        var testEntry = new TestEntry { ModifiedBy = "Admin" };

        Assert.Equal("Admin", testEntry.ModifiedBy);
    }

    [Fact]
    public void TestEntry_SetId_UpdatesId()
    {
        var testEntry = new TestEntry { Id = 1 };

        Assert.Equal(1, testEntry.Id);
    }

    [Fact]
    public void TestEntry_WithAllProperties_SetsCorrectly()
    {
        var testDate = DateTime.UtcNow.AddDays(-1);
        var createdDate = DateTime.UtcNow.AddDays(-2);
        var modifiedDate = DateTime.UtcNow;

        var testEntry = new TestEntry
        {
            Id = 1,
            BillNo = "BILL123",
            PatientName = "Jane Smith",
            PatientAge = 28,
            PatientGender = "Female",
            ContactNumber = "555-1234",
            TestSetupId = 5,
            TestDate = testDate,
            TotalFee = 750.00m,
            IsPaid = true,
            CreatedDate = createdDate,
            ModifiedDate = modifiedDate,
            CreatedBy = "Receptionist",
            ModifiedBy = "Admin"
        };

        Assert.Equal(1, testEntry.Id);
        Assert.Equal("BILL123", testEntry.BillNo);
        Assert.Equal("Jane Smith", testEntry.PatientName);
        Assert.Equal(28, testEntry.PatientAge);
        Assert.Equal("Female", testEntry.PatientGender);
        Assert.Equal("555-1234", testEntry.ContactNumber);
        Assert.Equal(5, testEntry.TestSetupId);
        Assert.Equal(testDate, testEntry.TestDate);
        Assert.Equal(750.00m, testEntry.TotalFee);
        Assert.True(testEntry.IsPaid);
        Assert.Equal(createdDate, testEntry.CreatedDate);
        Assert.Equal(modifiedDate, testEntry.ModifiedDate);
        Assert.Equal("Receptionist", testEntry.CreatedBy);
        Assert.Equal("Admin", testEntry.ModifiedBy);
    }

    [Fact]
    public void TestEntry_WithNullContactNumber_AllowsNull()
    {
        var testEntry = new TestEntry { ContactNumber = null };

        Assert.Null(testEntry.ContactNumber);
    }

    [Fact]
    public void TestEntry_WithNullModifiedDate_AllowsNull()
    {
        var testEntry = new TestEntry { ModifiedDate = null };

        Assert.Null(testEntry.ModifiedDate);
    }

    [Fact]
    public void TestEntry_IsPaidDefault_IsFalse()
    {
        var testEntry = new TestEntry();

        Assert.False(testEntry.IsPaid);
    }

    [Fact]
    public void TestEntry_WithZeroAge_AllowsZero()
    {
        var testEntry = new TestEntry { PatientAge = 0 };

        Assert.Equal(0, testEntry.PatientAge);
    }

    [Fact]
    public void TestEntry_WithNegativeFee_AllowsNegative()
    {
        var testEntry = new TestEntry { TotalFee = -100m };

        Assert.Equal(-100m, testEntry.TotalFee);
    }

    [Fact]
    public void TestEntry_Payments_CanAddPayment()
    {
        var testEntry = new TestEntry { Id = 1 };
        var payment = new Payment { Id = 1, TestEntryId = 1, Amount = 100m };
        testEntry.Payments.Add(payment);

        Assert.Single(testEntry.Payments);
        Assert.Contains(payment, testEntry.Payments);
    }

    [Fact]
    public void TestEntry_Payments_CanAddMultiplePayments()
    {
        var testEntry = new TestEntry { Id = 1 };
        var payment1 = new Payment { Id = 1, TestEntryId = 1, Amount = 100m };
        var payment2 = new Payment { Id = 2, TestEntryId = 1, Amount = 50m };
        testEntry.Payments.Add(payment1);
        testEntry.Payments.Add(payment2);

        Assert.Equal(2, testEntry.Payments.Count);
        Assert.Contains(payment1, testEntry.Payments);
        Assert.Contains(payment2, testEntry.Payments);
    }

    [Fact]
    public void TestEntry_Payments_InitializesAsEmptyList()
    {
        var testEntry = new TestEntry();

        Assert.NotNull(testEntry.Payments);
        Assert.Empty(testEntry.Payments);
        Assert.IsAssignableFrom<ICollection<Payment>>(testEntry.Payments);
    }
}
