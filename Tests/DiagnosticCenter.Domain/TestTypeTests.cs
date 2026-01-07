using Xunit;
using DiagnosticCenter.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests.DiagnosticCenter.Domain;

public class TestTypeTests
{
    [Fact]
    public void TestType_Constructor_InitializesWithDefaultValues()
    {
        var testType = new TestType();

        Assert.Equal(0, testType.Id);
        Assert.Equal(string.Empty, testType.Name);
        Assert.Null(testType.Description);
        Assert.True(testType.IsActive);
        Assert.Equal("System", testType.CreatedBy);
        Assert.Null(testType.ModifiedBy);
        Assert.NotNull(testType.TestSetups);
        Assert.Empty(testType.TestSetups);
    }

    [Fact]
    public void TestType_SetName_UpdatesName()
    {
        var testType = new TestType { Name = "Blood Test" };

        Assert.Equal("Blood Test", testType.Name);
    }

    [Fact]
    public void TestType_SetDescription_UpdatesDescription()
    {
        var testType = new TestType { Description = "Comprehensive blood analysis" };

        Assert.Equal("Comprehensive blood analysis", testType.Description);
    }

    [Fact]
    public void TestType_SetIsActive_UpdatesIsActive()
    {
        var testType = new TestType { IsActive = false };

        Assert.False(testType.IsActive);
    }

    [Fact]
    public void TestType_SetCreatedDate_UpdatesCreatedDate()
    {
        var date = DateTime.UtcNow.AddDays(-1);
        var testType = new TestType { CreatedDate = date };

        Assert.Equal(date, testType.CreatedDate);
    }

    [Fact]
    public void TestType_SetModifiedDate_UpdatesModifiedDate()
    {
        var date = DateTime.UtcNow;
        var testType = new TestType { ModifiedDate = date };

        Assert.Equal(date, testType.ModifiedDate);
    }

    [Fact]
    public void TestType_SetCreatedBy_UpdatesCreatedBy()
    {
        var testType = new TestType { CreatedBy = "Admin" };

        Assert.Equal("Admin", testType.CreatedBy);
    }

    [Fact]
    public void TestType_SetModifiedBy_UpdatesModifiedBy()
    {
        var testType = new TestType { ModifiedBy = "Technician" };

        Assert.Equal("Technician", testType.ModifiedBy);
    }

    [Fact]
    public void TestType_SetId_UpdatesId()
    {
        var testType = new TestType { Id = 1 };

        Assert.Equal(1, testType.Id);
    }

    [Fact]
    public void TestType_WithAllProperties_SetsCorrectly()
    {
        var createdDate = DateTime.UtcNow.AddDays(-2);
        var modifiedDate = DateTime.UtcNow.AddDays(-1);

        var testType = new TestType
        {
            Id = 1,
            Name = "X-Ray",
            Description = "Radiographic imaging",
            IsActive = true,
            CreatedDate = createdDate,
            ModifiedDate = modifiedDate,
            CreatedBy = "System",
            ModifiedBy = "Admin"
        };

        Assert.Equal(1, testType.Id);
        Assert.Equal("X-Ray", testType.Name);
        Assert.Equal("Radiographic imaging", testType.Description);
        Assert.True(testType.IsActive);
        Assert.Equal(createdDate, testType.CreatedDate);
        Assert.Equal(modifiedDate, testType.ModifiedDate);
        Assert.Equal("System", testType.CreatedBy);
        Assert.Equal("Admin", testType.ModifiedBy);
    }

    [Fact]
    public void TestType_WithNullDescription_AllowsNull()
    {
        var testType = new TestType { Description = null };

        Assert.Null(testType.Description);
    }

    [Fact]
    public void TestType_WithNullModifiedDate_AllowsNull()
    {
        var testType = new TestType { ModifiedDate = null };

        Assert.Null(testType.ModifiedDate);
    }

    [Fact]
    public void TestType_IsActiveDefault_IsTrue()
    {
        var testType = new TestType();

        Assert.True(testType.IsActive);
    }

    [Fact]
    public void TestType_TestSetups_CanAddTestSetup()
    {
        var testType = new TestType { Id = 1 };
        var testSetup = new TestSetup { Id = 1, Name = "Setup1", TypeId = 1 };
        testType.TestSetups.Add(testSetup);

        Assert.Single(testType.TestSetups);
        Assert.Contains(testSetup, testType.TestSetups);
    }

    [Fact]
    public void TestType_TestSetups_CanAddMultipleTestSetups()
    {
        var testType = new TestType { Id = 1 };
        var testSetup1 = new TestSetup { Id = 1, Name = "Setup1", TypeId = 1 };
        var testSetup2 = new TestSetup { Id = 2, Name = "Setup2", TypeId = 1 };
        testType.TestSetups.Add(testSetup1);
        testType.TestSetups.Add(testSetup2);

        Assert.Equal(2, testType.TestSetups.Count);
        Assert.Contains(testSetup1, testType.TestSetups);
        Assert.Contains(testSetup2, testType.TestSetups);
    }

    [Fact]
    public void TestType_TestSetups_InitializesAsEmptyList()
    {
        var testType = new TestType();

        Assert.NotNull(testType.TestSetups);
        Assert.Empty(testType.TestSetups);
        Assert.IsAssignableFrom<ICollection<TestSetup>>(testType.TestSetups);
    }
}
