using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using DiagnosticCenter.Infrastructure.Repositories;
using DiagnosticCenter.Infrastructure.Data;
using DiagnosticCenter.Domain.Entities;

namespace DiagnosticCenterTests.Infrastructure.Repositories;

public class TestTypeRepositoryTests
{
    private readonly Mock<ILogger<TestTypeRepository>> _mockLogger = new();
    private readonly DbContextOptions<DiagnosticCenterDbContext> _options;

    public TestTypeRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<DiagnosticCenterDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task GetAllAsync_ReturnsActiveTestTypes()
    {
        using var context = new DiagnosticCenterDbContext(_options);
        context.TestTypes.AddRange(
            new TestType { Id = 1, Name = "Blood Test", IsActive = true },
            new TestType { Id = 2, Name = "Inactive Test", IsActive = false }
        );
        await context.SaveChangesAsync();

        var repository = new TestTypeRepository(context, _mockLogger.Object);
        var result = await repository.GetAllAsync();

        Assert.Single(result);
        Assert.All(result, t => Assert.True(t.IsActive));
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ReturnsTestType()
    {
        using var context = new DiagnosticCenterDbContext(_options);
        context.TestTypes.Add(new TestType { Id = 1, Name = "Blood Test", IsActive = true });
        await context.SaveChangesAsync();

        var repository = new TestTypeRepository(context, _mockLogger.Object);
        var result = await repository.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Blood Test", result.Name);
    }

    [Fact]
    public async Task AddAsync_AddsTestTypeSuccessfully()
    {
        using var context = new DiagnosticCenterDbContext(_options);
        var repository = new TestTypeRepository(context, _mockLogger.Object);
        var testType = new TestType { Name = "New Test", IsActive = true };

        var result = await repository.AddAsync(testType);

        Assert.True(result.Id > 0);
        Assert.Equal("New Test", result.Name);
    }

    [Fact]
    public async Task ExistsAsync_WithExistingName_ReturnsTrue()
    {
        using var context = new DiagnosticCenterDbContext(_options);
        context.TestTypes.Add(new TestType { Id = 1, Name = "Existing Test", IsActive = true });
        await context.SaveChangesAsync();

        var repository = new TestTypeRepository(context, _mockLogger.Object);
        var result = await repository.ExistsAsync("Existing Test");

        Assert.True(result);
    }

    [Fact]
    public async Task SearchAsync_WithMatchingTerm_ReturnsMatchingTestTypes()
    {
        using var context = new DiagnosticCenterDbContext(_options);
        context.TestTypes.AddRange(
            new TestType { Id = 1, Name = "Blood Test", IsActive = true },
            new TestType { Id = 2, Name = "Blood Count", IsActive = true },
            new TestType { Id = 3, Name = "X-Ray", IsActive = true }
        );
        await context.SaveChangesAsync();

        var repository = new TestTypeRepository(context, _mockLogger.Object);
        var result = await repository.SearchAsync("Blood");

        Assert.Equal(2, result.Count());
    }
}
