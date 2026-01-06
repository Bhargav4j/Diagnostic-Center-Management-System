using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Infrastructure.Data;
using DiagnosticCenter.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace DiagnosticCenter.IntegrationTests.Repositories;

public class TestTypeRepositoryTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly TestTypeRepository _repository;

    public TestTypeRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _repository = new TestTypeRepository(_context, NullLogger<TestTypeRepository>.Instance);
    }

    [Fact]
    public async Task AddAsync_AddsTestTypeToDatabase()
    {
        // Arrange
        var testType = new TestType
        {
            Name = "Blood Test",
            Description = "Complete blood count",
            IsActive = true,
            CreatedBy = "admin",
            CreatedDate = DateTime.UtcNow
        };

        // Act
        var result = await _repository.AddAsync(testType);
        await _context.SaveChangesAsync();

        // Assert
        result.Id.Should().BeGreaterThan(0);
        var savedTestType = await _context.TestTypes.FindAsync(result.Id);
        savedTestType.Should().NotBeNull();
        savedTestType!.Name.Should().Be("Blood Test");
    }

    [Fact]
    public async Task GetAllAsync_ReturnsOnlyActiveTestTypes()
    {
        // Arrange
        await _repository.AddAsync(new TestType { Name = "Test1", IsActive = true, CreatedBy = "admin" });
        await _repository.AddAsync(new TestType { Name = "Test2", IsActive = true, CreatedBy = "admin" });
        await _repository.AddAsync(new TestType { Name = "Test3", IsActive = false, CreatedBy = "admin" });
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.All(t => t.IsActive).Should().BeTrue();
    }

    [Fact]
    public async Task NameExistsAsync_WithExistingName_ReturnsTrue()
    {
        // Arrange
        await _repository.AddAsync(new TestType { Name = "Blood Test", IsActive = true, CreatedBy = "admin" });
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.NameExistsAsync("Blood Test");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task NameExistsAsync_WithNonExistingName_ReturnsFalse()
    {
        // Act
        var result = await _repository.NameExistsAsync("NonExistent Test");

        // Assert
        result.Should().BeFalse();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
