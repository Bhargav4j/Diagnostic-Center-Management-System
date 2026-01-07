using Xunit;
using DiagnosticCenter.Domain.Entities;
using System;

namespace Tests.DiagnosticCenter.Domain;

public class UserTests
{
    [Fact]
    public void User_Constructor_InitializesWithDefaultValues()
    {
        var user = new User();

        Assert.Equal(0, user.Id);
        Assert.Equal(string.Empty, user.Email);
        Assert.Equal(string.Empty, user.PasswordHash);
        Assert.Equal(string.Empty, user.AccountType);
        Assert.True(user.IsActive);
        Assert.Equal("System", user.CreatedBy);
        Assert.Null(user.ModifiedBy);
    }

    [Fact]
    public void User_SetEmail_UpdatesEmail()
    {
        var user = new User { Email = "test@example.com" };

        Assert.Equal("test@example.com", user.Email);
    }

    [Fact]
    public void User_SetPasswordHash_UpdatesPasswordHash()
    {
        var user = new User { PasswordHash = "hashedpassword123" };

        Assert.Equal("hashedpassword123", user.PasswordHash);
    }

    [Fact]
    public void User_SetAccountType_UpdatesAccountType()
    {
        var user = new User { AccountType = "Admin" };

        Assert.Equal("Admin", user.AccountType);
    }

    [Fact]
    public void User_SetIsActive_UpdatesIsActive()
    {
        var user = new User { IsActive = false };

        Assert.False(user.IsActive);
    }

    [Fact]
    public void User_SetCreatedDate_UpdatesCreatedDate()
    {
        var date = DateTime.UtcNow.AddDays(-1);
        var user = new User { CreatedDate = date };

        Assert.Equal(date, user.CreatedDate);
    }

    [Fact]
    public void User_SetModifiedDate_UpdatesModifiedDate()
    {
        var date = DateTime.UtcNow;
        var user = new User { ModifiedDate = date };

        Assert.Equal(date, user.ModifiedDate);
    }

    [Fact]
    public void User_SetCreatedBy_UpdatesCreatedBy()
    {
        var user = new User { CreatedBy = "Admin" };

        Assert.Equal("Admin", user.CreatedBy);
    }

    [Fact]
    public void User_SetModifiedBy_UpdatesModifiedBy()
    {
        var user = new User { ModifiedBy = "Admin" };

        Assert.Equal("Admin", user.ModifiedBy);
    }

    [Fact]
    public void User_SetId_UpdatesId()
    {
        var user = new User { Id = 1 };

        Assert.Equal(1, user.Id);
    }

    [Fact]
    public void User_WithAllProperties_SetsCorrectly()
    {
        var createdDate = DateTime.UtcNow.AddDays(-2);
        var modifiedDate = DateTime.UtcNow.AddDays(-1);

        var user = new User
        {
            Id = 1,
            Email = "admin@example.com",
            PasswordHash = "hash123",
            AccountType = "Accountant",
            IsActive = true,
            CreatedDate = createdDate,
            ModifiedDate = modifiedDate,
            CreatedBy = "System",
            ModifiedBy = "Admin"
        };

        Assert.Equal(1, user.Id);
        Assert.Equal("admin@example.com", user.Email);
        Assert.Equal("hash123", user.PasswordHash);
        Assert.Equal("Accountant", user.AccountType);
        Assert.True(user.IsActive);
        Assert.Equal(createdDate, user.CreatedDate);
        Assert.Equal(modifiedDate, user.ModifiedDate);
        Assert.Equal("System", user.CreatedBy);
        Assert.Equal("Admin", user.ModifiedBy);
    }

    [Fact]
    public void User_WithEmptyEmail_AllowsEmptyString()
    {
        var user = new User { Email = "" };

        Assert.Equal(string.Empty, user.Email);
    }

    [Fact]
    public void User_WithNullModifiedDate_AllowsNull()
    {
        var user = new User { ModifiedDate = null };

        Assert.Null(user.ModifiedDate);
    }

    [Fact]
    public void User_IsActiveDefault_IsTrue()
    {
        var user = new User();

        Assert.True(user.IsActive);
    }
}
