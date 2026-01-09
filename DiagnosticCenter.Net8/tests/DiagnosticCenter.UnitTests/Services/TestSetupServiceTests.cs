using DiagnosticCenter.Application.Services;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DiagnosticCenter.UnitTests.Services;

public class TestSetupServiceTests
{
    private readonly Mock<ITestSetupRepository> _repositoryMock;
    private readonly Mock<ILogger<TestSetupService>> _loggerMock;
    private readonly TestSetupService _service;

    public TestSetupServiceTests()
    {
        _repositoryMock = new Mock<ITestSetupRepository>();
        _loggerMock = new Mock<ILogger<TestSetupService>>();
        _service = new TestSetupService(_repositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllTestSetups()
    {
        var testSetups = new List<TestSetup>
        {
            new() { Id = 1, Name = "Test1", Fee = 100, TypeId = 1, IsActive = true },
            new() { Id = 2, Name = "Test2", Fee = 200, TypeId = 1, IsActive = true }
        };

        _repositoryMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(testSetups);

        var result = await _service.GetAllAsync();

        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(testSetups);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnTestSetup_WhenExists()
    {
        var testSetup = new TestSetup { Id = 1, Name = "Test1", Fee = 100, TypeId = 1, IsActive = true };

        _repositoryMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(testSetup);

        var result = await _service.GetByIdAsync(1);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(testSetup);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateTestSetup()
    {
        var testSetup = new TestSetup { Name = "New Test", Fee = 150, TypeId = 1 };

        _repositoryMock.Setup(r => r.ExistsByNameAsync(testSetup.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _repositoryMock.Setup(r => r.AddAsync(testSetup, It.IsAny<CancellationToken>()))
            .ReturnsAsync(testSetup);

        var result = await _service.CreateAsync(testSetup);

        result.Should().NotBeNull();
        result.CreatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        result.IsActive.Should().BeTrue();
    }
}
