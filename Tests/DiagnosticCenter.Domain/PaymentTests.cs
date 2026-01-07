using Xunit;
using DiagnosticCenter.Domain.Entities;
using System;

namespace Tests.DiagnosticCenter.Domain;

public class PaymentTests
{
    [Fact]
    public void Payment_Constructor_InitializesWithDefaultValues()
    {
        var payment = new Payment();

        Assert.Equal(0, payment.Id);
        Assert.Equal(0, payment.TestEntryId);
        Assert.Equal(0, payment.Amount);
        Assert.Equal(string.Empty, payment.PaymentMode);
        Assert.Null(payment.Notes);
        Assert.True(payment.IsActive);
        Assert.Equal("System", payment.CreatedBy);
        Assert.Null(payment.ModifiedBy);
    }

    [Fact]
    public void Payment_SetTestEntryId_UpdatesTestEntryId()
    {
        var payment = new Payment { TestEntryId = 1 };

        Assert.Equal(1, payment.TestEntryId);
    }

    [Fact]
    public void Payment_SetAmount_UpdatesAmount()
    {
        var payment = new Payment { Amount = 250.50m };

        Assert.Equal(250.50m, payment.Amount);
    }

    [Fact]
    public void Payment_SetPaymentDate_UpdatesPaymentDate()
    {
        var date = DateTime.UtcNow;
        var payment = new Payment { PaymentDate = date };

        Assert.Equal(date, payment.PaymentDate);
    }

    [Fact]
    public void Payment_SetPaymentMode_UpdatesPaymentMode()
    {
        var payment = new Payment { PaymentMode = "Credit Card" };

        Assert.Equal("Credit Card", payment.PaymentMode);
    }

    [Fact]
    public void Payment_SetNotes_UpdatesNotes()
    {
        var payment = new Payment { Notes = "Partial payment" };

        Assert.Equal("Partial payment", payment.Notes);
    }

    [Fact]
    public void Payment_SetIsActive_UpdatesIsActive()
    {
        var payment = new Payment { IsActive = false };

        Assert.False(payment.IsActive);
    }

    [Fact]
    public void Payment_SetCreatedDate_UpdatesCreatedDate()
    {
        var date = DateTime.UtcNow.AddDays(-1);
        var payment = new Payment { CreatedDate = date };

        Assert.Equal(date, payment.CreatedDate);
    }

    [Fact]
    public void Payment_SetModifiedDate_UpdatesModifiedDate()
    {
        var date = DateTime.UtcNow;
        var payment = new Payment { ModifiedDate = date };

        Assert.Equal(date, payment.ModifiedDate);
    }

    [Fact]
    public void Payment_SetCreatedBy_UpdatesCreatedBy()
    {
        var payment = new Payment { CreatedBy = "Accountant" };

        Assert.Equal("Accountant", payment.CreatedBy);
    }

    [Fact]
    public void Payment_SetModifiedBy_UpdatesModifiedBy()
    {
        var payment = new Payment { ModifiedBy = "Admin" };

        Assert.Equal("Admin", payment.ModifiedBy);
    }

    [Fact]
    public void Payment_SetId_UpdatesId()
    {
        var payment = new Payment { Id = 1 };

        Assert.Equal(1, payment.Id);
    }

    [Fact]
    public void Payment_WithAllProperties_SetsCorrectly()
    {
        var paymentDate = DateTime.UtcNow.AddDays(-1);
        var createdDate = DateTime.UtcNow.AddDays(-2);
        var modifiedDate = DateTime.UtcNow;

        var payment = new Payment
        {
            Id = 1,
            TestEntryId = 5,
            Amount = 500.00m,
            PaymentDate = paymentDate,
            PaymentMode = "Cash",
            Notes = "Full payment received",
            IsActive = true,
            CreatedDate = createdDate,
            ModifiedDate = modifiedDate,
            CreatedBy = "Accountant",
            ModifiedBy = "Admin"
        };

        Assert.Equal(1, payment.Id);
        Assert.Equal(5, payment.TestEntryId);
        Assert.Equal(500.00m, payment.Amount);
        Assert.Equal(paymentDate, payment.PaymentDate);
        Assert.Equal("Cash", payment.PaymentMode);
        Assert.Equal("Full payment received", payment.Notes);
        Assert.True(payment.IsActive);
        Assert.Equal(createdDate, payment.CreatedDate);
        Assert.Equal(modifiedDate, payment.ModifiedDate);
        Assert.Equal("Accountant", payment.CreatedBy);
        Assert.Equal("Admin", payment.ModifiedBy);
    }

    [Fact]
    public void Payment_WithNullNotes_AllowsNull()
    {
        var payment = new Payment { Notes = null };

        Assert.Null(payment.Notes);
    }

    [Fact]
    public void Payment_WithNullModifiedDate_AllowsNull()
    {
        var payment = new Payment { ModifiedDate = null };

        Assert.Null(payment.ModifiedDate);
    }

    [Fact]
    public void Payment_IsActiveDefault_IsTrue()
    {
        var payment = new Payment();

        Assert.True(payment.IsActive);
    }

    [Fact]
    public void Payment_WithZeroAmount_AllowsZero()
    {
        var payment = new Payment { Amount = 0m };

        Assert.Equal(0m, payment.Amount);
    }

    [Fact]
    public void Payment_WithNegativeAmount_AllowsNegative()
    {
        var payment = new Payment { Amount = -100m };

        Assert.Equal(-100m, payment.Amount);
    }

    [Fact]
    public void Payment_WithDifferentPaymentModes_SetsCorrectly()
    {
        var payment1 = new Payment { PaymentMode = "Cash" };
        var payment2 = new Payment { PaymentMode = "Credit Card" };
        var payment3 = new Payment { PaymentMode = "Debit Card" };
        var payment4 = new Payment { PaymentMode = "Online Transfer" };

        Assert.Equal("Cash", payment1.PaymentMode);
        Assert.Equal("Credit Card", payment2.PaymentMode);
        Assert.Equal("Debit Card", payment3.PaymentMode);
        Assert.Equal("Online Transfer", payment4.PaymentMode);
    }

    [Fact]
    public void Payment_WithLargeAmount_HandlesBigDecimal()
    {
        var payment = new Payment { Amount = 999999.99m };

        Assert.Equal(999999.99m, payment.Amount);
    }

    [Fact]
    public void Payment_WithEmptyPaymentMode_AllowsEmptyString()
    {
        var payment = new Payment { PaymentMode = "" };

        Assert.Equal(string.Empty, payment.PaymentMode);
    }
}
