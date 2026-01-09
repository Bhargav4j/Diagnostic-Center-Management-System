using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Infrastructure.Data;
using DiagnosticCenter.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiagnosticCenter.UnitTests.Infrastructure.Repositories;

public class TestTypeRepositoryTests : IDisposable
{
    private readonly DiagnosticCenterDbContext _context;
    private readonly Mock<ILogger<TestTypeRepository>> _mockLogger;
    private readonly TestTypeRepository _repository;

    public TestTypeRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<DiagnosticCenterDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new DiagnosticCenterDbContext(options);
        _mockLogger = new Mock<ILogger<TestTypeRepository>>();
        _repository = new TestTypeRepository(_context, _mockLogger.Object);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenContextIsNull()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() => new TestTypeRepository(null!, _mockLogger.Object));
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() => new TestTypeRepository(_context, null!));
    }

    [Fact]
    public void Constructor_ShouldCreateInstance_WhenValidParametersProvided()
    {
        // Arrange & Act
        var repository = new TestTypeRepository(_context, _mockLogger.Object);

        // Assert
        Assert.NotNull(repository);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllActiveTestTypes()
    {
        // Arrange
        await _context.TestTypes.AddRangeAsync(
            new TestType { Id = 1, Name = "Blood Test", IsActive = true },
            new TestType { Id = 2, Name = "X-Ray", IsActive = true },
            new TestType { Id = 3, Name = "Inactive Test", IsActive = false }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.All(result, t => Assert.True(t.IsActive));
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoActiveTestTypes()
    {
        // Arrange
        await _context.TestTypes.AddAsync(new TestType { Id = 1, Name = "Inactive", IsActive = false });
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnTestType_WhenActiveTestTypeExists()
    {
        // Arrange
        var testType = new TestType { Id = 1, Name = "Blood Test", IsActive = true };
        await _context.TestTypes.AddAsync(testType);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Blood Test", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenTestTypeNotFound()
    {
        // Act
        var result = await _repository.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenTestTypeIsInactive()
    {
        // Arrange
        var testType = new TestType { Id = 1, Name = "Inactive Test", IsActive = false };
        await _context.TestTypes.AddAsync(testType);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_ShouldAddNewTestType()
    {
        // Arrange
        var testType = new TestType { Name = "New Test", IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "Admin" };

        // Act
        var result = await _repository.AddAsync(testType);

        // Assert
        Assert.NotNull(result);
        Assert.NotEqual(0, result.Id);
        Assert.Equal("New Test", result.Name);

        var saved = await _context.TestTypes.FindAsync(result.Id);
        Assert.NotNull(saved);
        Assert.Equal("New Test", saved.Name);
    }

    [Fact]
    public async Task AddAsync_ShouldReturnTestTypeWithGeneratedId()
    {
        // Arrange
        var testType = new TestType { Name = "Test", CreatedDate = DateTime.UtcNow, CreatedBy = "Admin" };

        // Act
        var result = await _repository.AddAsync(testType);

        // Assert
        Assert.True(result.Id > 0);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateExistingTestType()
    {
        // Arrange
        var testType = new TestType { Name = "Original Name", IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "Admin" };
        await _context.TestTypes.AddAsync(testType);
        await _context.SaveChangesAsync();

        testType.Name = "Updated Name";
        testType.ModifiedDate = DateTime.UtcNow;

        // Act
        await _repository.UpdateAsync(testType);

        // Assert
        var updated = await _context.TestTypes.FindAsync(testType.Id);
        Assert.NotNull(updated);
        Assert.Equal("Updated Name", updated.Name);
        Assert.NotNull(updated.ModifiedDate);
    }

    [Fact]
    public async Task DeleteAsync_ShouldSetIsActiveToFalse()
    {
        // Arrange
        var testType = new TestType { Name = "Test", IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "Admin" };
        await _context.TestTypes.AddAsync(testType);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(testType.Id);

        // Assert
        var deleted = await _context.TestTypes.FindAsync(testType.Id);
        Assert.NotNull(deleted);
        Assert.False(deleted.IsActive);
        Assert.NotNull(deleted.ModifiedDate);
    }

    [Fact]
    public async Task DeleteAsync_ShouldNotThrowException_WhenTestTypeNotFound()
    {
        // Act & Assert
        await _repository.DeleteAsync(999);
        // No exception should be thrown
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrue_WhenActiveTestTypeExists()
    {
        // Arrange
        var testType = new TestType { Id = 1, Name = "Test", IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "Admin" };
        await _context.TestTypes.AddAsync(testType);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.ExistsAsync(1);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnFalse_WhenTestTypeNotFound()
    {
        // Act
        var result = await _repository.ExistsAsync(999);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnFalse_WhenTestTypeIsInactive()
    {
        // Arrange
        var testType = new TestType { Id = 1, Name = "Inactive", IsActive = false, CreatedDate = DateTime.UtcNow, CreatedBy = "Admin" };
        await _context.TestTypes.AddAsync(testType);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.ExistsAsync(1);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task SearchAsync_ShouldReturnMatchingTestTypes()
    {
        // Arrange
        await _context.TestTypes.AddRangeAsync(
            new TestType { Name = "Blood Test", IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "Admin" },
            new TestType { Name = "Blood Sugar", IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "Admin" },
            new TestType { Name = "X-Ray", IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "Admin" }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.SearchAsync("Blood");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.All(result, t => Assert.Contains("Blood", t.Name));
    }

    [Fact]
    public async Task SearchAsync_ShouldReturnEmptyList_WhenNoMatches()
    {
        // Arrange
        await _context.TestTypes.AddAsync(new TestType { Name = "X-Ray", IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "Admin" });
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.SearchAsync("NonExistent");

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task SearchAsync_ShouldNotReturnInactiveTestTypes()
    {
        // Arrange
        await _context.TestTypes.AddRangeAsync(
            new TestType { Name = "Blood Test Active", IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "Admin" },
            new TestType { Name = "Blood Test Inactive", IsActive = false, CreatedDate = DateTime.UtcNow, CreatedBy = "Admin" }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.SearchAsync("Blood");

        // Assert
        Assert.Single(result);
        Assert.Contains(result, t => t.Name == "Blood Test Active");
    }

    [Fact]
    public async Task GetAllAsync_ShouldSupportCancellationToken()
    {
        // Arrange
        var cts = new CancellationTokenSource();

        // Act
        var result = await _repository.GetAllAsync(cts.Token);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldSupportCancellationToken()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        var testType = new TestType { Name = "Test", IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "Admin" };
        await _context.TestTypes.AddAsync(testType);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(testType.Id, cts.Token);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task AddAsync_ShouldSupportCancellationToken()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        var testType = new TestType { Name = "Test", CreatedDate = DateTime.UtcNow, CreatedBy = "Admin" };

        // Act
        var result = await _repository.AddAsync(testType, cts.Token);

        // Assert
        Assert.NotNull(result);
        Assert.NotEqual(0, result.Id);
    }

    [Fact]
    public async Task UpdateAsync_ShouldSupportCancellationToken()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        var testType = new TestType { Name = "Test", IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "Admin" };
        await _context.TestTypes.AddAsync(testType);
        await _context.SaveChangesAsync();

        testType.Name = "Updated";

        // Act
        await _repository.UpdateAsync(testType, cts.Token);

        // Assert
        var updated = await _context.TestTypes.FindAsync(testType.Id);
        Assert.Equal("Updated", updated!.Name);
    }

    [Fact]
    public async Task DeleteAsync_ShouldSupportCancellationToken()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        var testType = new TestType { Name = "Test", IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "Admin" };
        await _context.TestTypes.AddAsync(testType);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(testType.Id, cts.Token);

        // Assert
        var deleted = await _context.TestTypes.FindAsync(testType.Id);
        Assert.False(deleted!.IsActive);
    }

    [Fact]
    public async Task ExistsAsync_ShouldSupportCancellationToken()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        var testType = new TestType { Name = "Test", IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "Admin" };
        await _context.TestTypes.AddAsync(testType);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.ExistsAsync(testType.Id, cts.Token);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task SearchAsync_ShouldSupportCancellationToken()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        await _context.TestTypes.AddAsync(new TestType { Name = "Blood Test", IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "Admin" });
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.SearchAsync("Blood", cts.Token);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
    }
}
