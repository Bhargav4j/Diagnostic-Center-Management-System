using Xunit;
using DiagnosticCenter.Infrastructure.Data;
using DiagnosticCenter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace DiagnosticCenter.Infrastructure.Data.Tests;

public class DiagnosticCenterDbContextTests
{
    private readonly DbContextOptions<DiagnosticCenterDbContext> _options;

    public DiagnosticCenterDbContextTests()
    {
        _options = new DbContextOptionsBuilder<DiagnosticCenterDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public void Constructor_ShouldInitializeContext()
    {
        // Arrange & Act
        using var context = new DiagnosticCenterDbContext(_options);

        // Assert
        Assert.NotNull(context);
    }

    [Fact]
    public void TestTypes_DbSet_ShouldBeAccessible()
    {
        // Arrange
        using var context = new DiagnosticCenterDbContext(_options);

        // Act
        var dbSet = context.TestTypes;

        // Assert
        Assert.NotNull(dbSet);
    }

    [Fact]
    public void TestSetups_DbSet_ShouldBeAccessible()
    {
        // Arrange
        using var context = new DiagnosticCenterDbContext(_options);

        // Act
        var dbSet = context.TestSetups;

        // Assert
        Assert.NotNull(dbSet);
    }

    [Fact]
    public void TestEntries_DbSet_ShouldBeAccessible()
    {
        // Arrange
        using var context = new DiagnosticCenterDbContext(_options);

        // Act
        var dbSet = context.TestEntries;

        // Assert
        Assert.NotNull(dbSet);
    }

    [Fact]
    public void Payments_DbSet_ShouldBeAccessible()
    {
        // Arrange
        using var context = new DiagnosticCenterDbContext(_options);

        // Act
        var dbSet = context.Payments;

        // Assert
        Assert.NotNull(dbSet);
    }

    [Fact]
    public void Users_DbSet_ShouldBeAccessible()
    {
        // Arrange
        using var context = new DiagnosticCenterDbContext(_options);

        // Act
        var dbSet = context.Users;

        // Assert
        Assert.NotNull(dbSet);
    }

    [Fact]
    public void SaveChangesAsync_ShouldPersistData()
    {
        // Arrange
        using var context = new DiagnosticCenterDbContext(_options);
        var testType = new TestType { Name = "Test", IsActive = true, CreatedBy = "User" };

        // Act
        context.TestTypes.Add(testType);
        var result = context.SaveChangesAsync().Result;

        // Assert
        Assert.True(result > 0);
    }

    [Fact]
    public void Context_CanAddTestType_AndRetrieve()
    {
        // Arrange
        using var context = new DiagnosticCenterDbContext(_options);
        var testType = new TestType { Name = "Blood Test", IsActive = true, CreatedBy = "Admin" };

        // Act
        context.TestTypes.Add(testType);
        context.SaveChanges();
        var retrieved = context.TestTypes.FirstOrDefault(t => t.Name == "Blood Test");

        // Assert
        Assert.NotNull(retrieved);
        Assert.Equal("Blood Test", retrieved.Name);
    }

    [Fact]
    public void Context_CanAddUser_AndRetrieve()
    {
        // Arrange
        using var context = new DiagnosticCenterDbContext(_options);
        var user = new User { Email = "test@example.com", PasswordHash = "hash", AccountType = "Admin", IsActive = true };

        // Act
        context.Users.Add(user);
        context.SaveChanges();
        var retrieved = context.Users.FirstOrDefault(u => u.Email == "test@example.com");

        // Assert
        Assert.NotNull(retrieved);
        Assert.Equal("test@example.com", retrieved.Email);
    }
}
