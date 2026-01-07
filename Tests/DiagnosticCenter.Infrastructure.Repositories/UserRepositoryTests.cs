using Xunit;
using Moq;
using DiagnosticCenter.Infrastructure.Repositories;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DiagnosticCenter.Infrastructure.Repositories.Tests;

public class UserRepositoryTests
{
    private readonly Mock<ILogger<UserRepository>> _mockLogger;
    private readonly DbContextOptions<DiagnosticCenterDbContext> _options;

    public UserRepositoryTests()
    {
        _mockLogger = new Mock<ILogger<UserRepository>>();
        _options = new DbContextOptionsBuilder<DiagnosticCenterDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public void Constructor_WithNullContext_ShouldThrowArgumentNullException()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() => new UserRepository(null!, _mockLogger.Object));
    }

    [Fact]
    public void Constructor_WithNullLogger_ShouldThrowArgumentNullException()
    {
        // Arrange
        using var context = new DiagnosticCenterDbContext(_options);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new UserRepository(context, null!));
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnActiveUsers()
    {
        // Arrange
        using var context = new DiagnosticCenterDbContext(_options);
        context.Users.AddRange(
            new User { Id = 1, Email = "user1@test.com", PasswordHash = "hash", AccountType = "Admin", IsActive = true },
            new User { Id = 2, Email = "user2@test.com", PasswordHash = "hash", AccountType = "User", IsActive = true },
            new User { Id = 3, Email = "user3@test.com", PasswordHash = "hash", AccountType = "User", IsActive = false }
        );
        await context.SaveChangesAsync();

        var repository = new UserRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.All(result, u => Assert.True(u.IsActive));
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ShouldReturnUser()
    {
        // Arrange
        using var context = new DiagnosticCenterDbContext(_options);
        var user = new User { Id = 1, Email = "test@test.com", PasswordHash = "hash", AccountType = "Admin", IsActive = true };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var repository = new UserRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        using var context = new DiagnosticCenterDbContext(_options);
        var repository = new UserRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByEmailAsync_WithValidEmail_ShouldReturnUser()
    {
        // Arrange
        using var context = new DiagnosticCenterDbContext(_options);
        var user = new User { Email = "test@test.com", PasswordHash = "hash", AccountType = "Admin", IsActive = true };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var repository = new UserRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.GetByEmailAsync("test@test.com");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test@test.com", result.Email);
    }

    [Fact]
    public async Task GetByEmailAsync_WithInvalidEmail_ShouldReturnNull()
    {
        // Arrange
        using var context = new DiagnosticCenterDbContext(_options);
        var repository = new UserRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.GetByEmailAsync("nonexistent@test.com");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_ShouldAddUser()
    {
        // Arrange
        using var context = new DiagnosticCenterDbContext(_options);
        var repository = new UserRepository(context, _mockLogger.Object);
        var user = new User { Email = "newuser@test.com", PasswordHash = "hash", AccountType = "User", IsActive = true };

        // Act
        var result = await repository.AddAsync(user);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal("newuser@test.com", result.Email);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateUser()
    {
        // Arrange
        using var context = new DiagnosticCenterDbContext(_options);
        var user = new User { Id = 1, Email = "old@test.com", PasswordHash = "hash", AccountType = "User", IsActive = true };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var repository = new UserRepository(context, _mockLogger.Object);
        user.Email = "new@test.com";

        // Act
        await repository.UpdateAsync(user);

        // Assert
        var updated = await context.Users.FindAsync(1);
        Assert.NotNull(updated);
        Assert.Equal("new@test.com", updated.Email);
    }

    [Fact]
    public async Task DeleteAsync_ShouldSoftDeleteUser()
    {
        // Arrange
        using var context = new DiagnosticCenterDbContext(_options);
        var user = new User { Id = 1, Email = "test@test.com", PasswordHash = "hash", AccountType = "User", IsActive = true };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var repository = new UserRepository(context, _mockLogger.Object);

        // Act
        await repository.DeleteAsync(1);

        // Assert
        var deleted = await context.Users.FindAsync(1);
        Assert.NotNull(deleted);
        Assert.False(deleted.IsActive);
    }

    [Fact]
    public async Task ExistsAsync_WhenExists_ShouldReturnTrue()
    {
        // Arrange
        using var context = new DiagnosticCenterDbContext(_options);
        var user = new User { Id = 1, Email = "test@test.com", PasswordHash = "hash", AccountType = "User", IsActive = true };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var repository = new UserRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.ExistsAsync(1);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ExistsAsync_WhenNotExists_ShouldReturnFalse()
    {
        // Arrange
        using var context = new DiagnosticCenterDbContext(_options);
        var repository = new UserRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.ExistsAsync(999);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task SearchAsync_ShouldReturnMatchingUsers()
    {
        // Arrange
        using var context = new DiagnosticCenterDbContext(_options);
        context.Users.AddRange(
            new User { Email = "admin@test.com", PasswordHash = "hash", AccountType = "Admin", FullName = "Admin User", IsActive = true },
            new User { Email = "user@test.com", PasswordHash = "hash", AccountType = "User", FullName = "Regular User", IsActive = true },
            new User { Email = "test@test.com", PasswordHash = "hash", AccountType = "User", FullName = "Test Admin", IsActive = true }
        );
        await context.SaveChangesAsync();

        var repository = new UserRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.SearchAsync("Admin");

        // Assert
        Assert.Equal(2, result.Count());
    }
}
