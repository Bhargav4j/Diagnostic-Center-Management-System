using Xunit;
using DiagnosticCenter.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Tests.DiagnosticCenter.Domain;

public class TestSetupTests
{
    [Fact]
    public void TestSetup_Constructor_InitializesWithDefaultValues()
    {
        var testSetup = new TestSetup();

        Assert.Equal(0, testSetup.Id);
        Assert.Equal(string.Empty, testSetup.Name);
        Assert.Equal(0, testSetup.Fee);
        Assert.Equal(0, testSetup.TypeId);
        Assert.True(testSetup.IsActive);
        Assert.Equal("System", testSetup.CreatedBy);
        Assert.Null(testSetup.ModifiedBy);
        Assert.NotNull(testSetup.TestEntries);
        Assert.Empty(testSetup.TestEntries);
    }

    [Fact]
    public void TestSetup_SetName_UpdatesName()
    {
        var testSetup = new TestSetup { Name = "CBC Test" };

        Assert.Equal("CBC Test", testSetup.Name);
    }

    [Fact]
    public void TestSetup_SetFee_UpdatesFee()
    {
        var testSetup = new TestSetup { Fee = 150.50m };

        Assert.Equal(150.50m, testSetup.Fee);
    }

    [Fact]
    public void TestSetup_SetTypeId_UpdatesTypeId()
    {
        var testSetup = new TestSetup { TypeId = 1 };

        Assert.Equal(1, testSetup.TypeId);
    }

    [Fact]
    public void TestSetup_SetIsActive_UpdatesIsActive()
    {
        var testSetup = new TestSetup { IsActive = false };

        Assert.False(testSetup.IsActive);
    }

    [Fact]
    public void TestSetup_SetCreatedDate_UpdatesCreatedDate()
    {
        var date = DateTime.UtcNow.AddDays(-1);
        var testSetup = new TestSetup { CreatedDate = date };

        Assert.Equal(date, testSetup.CreatedDate);
    }

    [Fact]
    public void TestSetup_SetModifiedDate_UpdatesModifiedDate()
    {
        var date = DateTime.UtcNow;
        var testSetup = new TestSetup { ModifiedDate = date };

        Assert.Equal(date, testSetup.ModifiedDate);
    }

    [Fact]
    public void TestSetup_SetCreatedBy_UpdatesCreatedBy()
    {
        var testSetup = new TestSetup { CreatedBy = "Admin" };

        Assert.Equal("Admin", testSetup.CreatedBy);
    }

    [Fact]
    public void TestSetup_SetModifiedBy_UpdatesModifiedBy()
    {
        var testSetup = new TestSetup { ModifiedBy = "Technician" };

        Assert.Equal("Technician", testSetup.ModifiedBy);
    }

    [Fact]
    public void TestSetup_SetId_UpdatesId()
    {
        var testSetup = new TestSetup { Id = 1 };

        Assert.Equal(1, testSetup.Id);
    }

    [Fact]
    public void TestSetup_WithAllProperties_SetsCorrectly()
    {
        var createdDate = DateTime.UtcNow.AddDays(-2);
        var modifiedDate = DateTime.UtcNow.AddDays(-1);

        var testSetup = new TestSetup
        {
            Id = 1,
            Name = "Lipid Profile",
            Fee = 200.00m,
            TypeId = 5,
            IsActive = true,
            CreatedDate = createdDate,
            ModifiedDate = modifiedDate,
            CreatedBy = "System",
            ModifiedBy = "Admin"
        };

        Assert.Equal(1, testSetup.Id);
        Assert.Equal("Lipid Profile", testSetup.Name);
        Assert.Equal(200.00m, testSetup.Fee);
        Assert.Equal(5, testSetup.TypeId);
        Assert.True(testSetup.IsActive);
        Assert.Equal(createdDate, testSetup.CreatedDate);
        Assert.Equal(modifiedDate, testSetup.ModifiedDate);
        Assert.Equal("System", testSetup.CreatedBy);
        Assert.Equal("Admin", testSetup.ModifiedBy);
    }

    [Fact]
    public void TestSetup_WithNullModifiedDate_AllowsNull()
    {
        var testSetup = new TestSetup { ModifiedDate = null };

        Assert.Null(testSetup.ModifiedDate);
    }

    [Fact]
    public void TestSetup_IsActiveDefault_IsTrue()
    {
        var testSetup = new TestSetup();

        Assert.True(testSetup.IsActive);
    }

    [Fact]
    public void TestSetup_WithZeroFee_AllowsZero()
    {
        var testSetup = new TestSetup { Fee = 0m };

        Assert.Equal(0m, testSetup.Fee);
    }

    [Fact]
    public void TestSetup_WithNegativeFee_AllowsNegative()
    {
        var testSetup = new TestSetup { Fee = -50m };

        Assert.Equal(-50m, testSetup.Fee);
    }

    [Fact]
    public void TestSetup_TestEntries_CanAddTestEntry()
    {
        var testSetup = new TestSetup { Id = 1 };
        var testEntry = new TestEntry { Id = 1, TestSetupId = 1 };
        testSetup.TestEntries.Add(testEntry);

        Assert.Single(testSetup.TestEntries);
        Assert.Contains(testEntry, testSetup.TestEntries);
    }

    [Fact]
    public void TestSetup_TestEntries_CanAddMultipleTestEntries()
    {
        var testSetup = new TestSetup { Id = 1 };
        var testEntry1 = new TestEntry { Id = 1, TestSetupId = 1 };
        var testEntry2 = new TestEntry { Id = 2, TestSetupId = 1 };
        testSetup.TestEntries.Add(testEntry1);
        testSetup.TestEntries.Add(testEntry2);

        Assert.Equal(2, testSetup.TestEntries.Count);
        Assert.Contains(testEntry1, testSetup.TestEntries);
        Assert.Contains(testEntry2, testSetup.TestEntries);
    }

    [Fact]
    public void TestSetup_TestEntries_InitializesAsEmptyList()
    {
        var testSetup = new TestSetup();

        Assert.NotNull(testSetup.TestEntries);
        Assert.Empty(testSetup.TestEntries);
        Assert.IsAssignableFrom<ICollection<TestEntry>>(testSetup.TestEntries);
    }

    [Fact]
    public void TestSetup_WithLargeFee_HandlesBigDecimal()
    {
        var testSetup = new TestSetup { Fee = 999999.99m };

        Assert.Equal(999999.99m, testSetup.Fee);
    }
}
