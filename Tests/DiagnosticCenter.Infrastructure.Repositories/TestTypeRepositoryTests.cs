using Xunit;
using Moq;
using DiagnosticCenter.Infrastructure.Repositories;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiagnosticCenter.Infrastructure.Repositories.Tests;

public class TestTypeRepositoryTests
{
    private readonly Mock<ILogger<TestTypeRepository>> _mockLogger;
    private readonly DbContextOptions<DiagnosticCenterDbContext> _options;

    public TestTypeRepositoryTests()
    {
        _mockLogger = new Mock<ILogger<TestTypeRepository>>();
        _options = new DbContextOptionsBuilder<DiagnosticCenterDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public void Constructor_WithNullContext_ShouldThrowArgumentNullException()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() => new TestTypeRepository(null!, _mockLogger.Object));
    }

    [Fact]
    public void Constructor_WithNullLogger_ShouldThrowArgumentNullException()
    {
        // Arrange
        using var context = new DiagnosticCenterDbContext(_options);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new TestTypeRepository(context, null!));
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnActiveTestTypes()
    {
        // Arrange
        using var context = new DiagnosticCenterDbContext(_options);
        context.TestTypes.AddRange(
            new TestType { Id = 1, Name = "Test1", IsActive = true, CreatedBy = "User" },
            new TestType { Id = 2, Name = "Test2", IsActive = true, CreatedBy = "User" },
            new TestType { Id = 3, Name = "Test3", IsActive = false, CreatedBy = "User" }
        );
        await context.SaveChangesAsync();

        var repository = new TestTypeRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.All(result, t => Assert.True(t.IsActive));
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ShouldReturnTestType()
    {
        // Arrange
        using var context = new DiagnosticCenterDbContext(_options);
        var testType = new TestType { Id = 1, Name = "Test1", IsActive = true, CreatedBy = "User" };
        context.TestTypes.Add(testType);
        await context.SaveChangesAsync();

        var repository = new TestTypeRepository(context, _mockLogger.Object);

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
        var repository = new TestTypeRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_ShouldAddTestType()
    {
        // Arrange
        using var context = new DiagnosticCenterDbContext(_options);
        var repository = new TestTypeRepository(context, _mockLogger.Object);
        var testType = new TestType { Name = "NewTest", IsActive = true, CreatedBy = "User" };

        // Act
        var result = await repository.AddAsync(testType);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal("NewTest", result.Name);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateTestType()
    {
        // Arrange
        using var context = new DiagnosticCenterDbContext(_options);
        var testType = new TestType { Id = 1, Name = "OldName", IsActive = true, CreatedBy = "User" };
        context.TestTypes.Add(testType);
        await context.SaveChangesAsync();

        var repository = new TestTypeRepository(context, _mockLogger.Object);
        testType.Name = "NewName";

        // Act
        await repository.UpdateAsync(testType);

        // Assert
        var updated = await context.TestTypes.FindAsync(1);
        Assert.NotNull(updated);
        Assert.Equal("NewName", updated.Name);
    }

    [Fact]
    public async Task DeleteAsync_ShouldSoftDeleteTestType()
    {
        // Arrange
        using var context = new DiagnosticCenterDbContext(_options);
        var testType = new TestType { Id = 1, Name = "Test", IsActive = true, CreatedBy = "User" };
        context.TestTypes.Add(testType);
        await context.SaveChangesAsync();

        var repository = new TestTypeRepository(context, _mockLogger.Object);

        // Act
        await repository.DeleteAsync(1);

        // Assert
        var deleted = await context.TestTypes.FindAsync(1);
        Assert.NotNull(deleted);
        Assert.False(deleted.IsActive);
    }

    [Fact]
    public async Task ExistsAsync_WhenExists_ShouldReturnTrue()
    {
        // Arrange
        using var context = new DiagnosticCenterDbContext(_options);
        var testType = new TestType { Id = 1, Name = "Test", IsActive = true, CreatedBy = "User" };
        context.TestTypes.Add(testType);
        await context.SaveChangesAsync();

        var repository = new TestTypeRepository(context, _mockLogger.Object);

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
        var repository = new TestTypeRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.ExistsAsync(999);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task ExistsByNameAsync_WhenExists_ShouldReturnTrue()
    {
        // Arrange
        using var context = new DiagnosticCenterDbContext(_options);
        var testType = new TestType { Name = "BloodTest", IsActive = true, CreatedBy = "User" };
        context.TestTypes.Add(testType);
        await context.SaveChangesAsync();

        var repository = new TestTypeRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.ExistsByNameAsync("BloodTest");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task SearchAsync_ShouldReturnMatchingTestTypes()
    {
        // Arrange
        using var context = new DiagnosticCenterDbContext(_options);
        context.TestTypes.AddRange(
            new TestType { Name = "Blood Test", IsActive = true, CreatedBy = "User" },
            new TestType { Name = "X-Ray Test", IsActive = true, CreatedBy = "User" },
            new TestType { Name = "Blood Sugar", IsActive = true, CreatedBy = "User" }
        );
        await context.SaveChangesAsync();

        var repository = new TestTypeRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.SearchAsync("Blood");

        // Assert
        Assert.Equal(2, result.Count());
    }
}
