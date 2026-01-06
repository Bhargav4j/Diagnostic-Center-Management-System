using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Infrastructure.Data;
using DiagnosticCenter.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        var logger = NullLogger<TestTypeRepository>.Instance;
        _repository = new TestTypeRepository(_context, logger);
    }

    [Fact]
    public async Task AddAsync_AddsTestTypeToDatabase()
    {
        var testType = new TestType
        {
            Name = "Blood Test",
            Description = "Complete blood count",
            CreatedBy = "admin",
            IsActive = true
        };

        var result = await _repository.AddAsync(testType);

        result.Id.Should().BeGreaterThan(0);
        var savedTestType = await _context.TestTypes.FindAsync(result.Id);
        savedTestType.Should().NotBeNull();
        savedTestType!.Name.Should().Be("Blood Test");
    }

    [Fact]
    public async Task GetAllAsync_ReturnsActiveTestTypes()
    {
        _context.TestTypes.AddRange(
            new TestType { Name = "Test1", CreatedBy = "admin", IsActive = true },
            new TestType { Name = "Test2", CreatedBy = "admin", IsActive = true },
            new TestType { Name = "Test3", CreatedBy = "admin", IsActive = false }
        );
        await _context.SaveChangesAsync();

        var result = await _repository.GetAllAsync();

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ReturnsTestType()
    {
        var testType = new TestType { Name = "X-Ray", CreatedBy = "admin", IsActive = true };
        _context.TestTypes.Add(testType);
        await _context.SaveChangesAsync();

        var result = await _repository.GetByIdAsync(testType.Id);

        result.Should().NotBeNull();
        result!.Name.Should().Be("X-Ray");
    }

    [Fact]
    public async Task ExistsByNameAsync_WithExistingName_ReturnsTrue()
    {
        var testType = new TestType { Name = "Unique Test", CreatedBy = "admin", IsActive = true };
        _context.TestTypes.Add(testType);
        await _context.SaveChangesAsync();

        var result = await _repository.ExistsByNameAsync("Unique Test");

        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_SoftDeletesTestType()
    {
        var testType = new TestType { Name = "To Delete", CreatedBy = "admin", IsActive = true };
        _context.TestTypes.Add(testType);
        await _context.SaveChangesAsync();

        await _repository.DeleteAsync(testType.Id);

        var deletedTestType = await _context.TestTypes.FindAsync(testType.Id);
        deletedTestType!.IsActive.Should().BeFalse();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
