using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FluentAssertions;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Infrastructure.Data;
using DiagnosticCenter.Infrastructure.Repositories;
using Moq;

namespace DiagnosticCenter.IntegrationTests.Repositories;

public class TestSetupRepositoryTests : IDisposable
{
    private readonly DiagnosticCenterDbContext _context;
    private readonly TestSetupRepository _repository;

    public TestSetupRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<DiagnosticCenterDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new DiagnosticCenterDbContext(options);
        var logger = new Mock<ILogger<TestSetupRepository>>();
        _repository = new TestSetupRepository(_context, logger.Object);
    }

    [Fact]
    public async Task AddAsync_AddsTestSetupToDatabase()
    {
        // Arrange
        var testType = new TestType { Id = 1, Name = "Type 1" };
        await _context.TestTypes.AddAsync(testType);
        await _context.SaveChangesAsync();

        var testSetup = new TestSetup
        {
            Name = "Test Setup 1",
            Fee = 100.50m,
            TypeId = 1
        };

        // Act
        var result = await _repository.AddAsync(testSetup);

        // Assert
        result.Id.Should().BeGreaterThan(0);
        result.Name.Should().Be("Test Setup 1");
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}
