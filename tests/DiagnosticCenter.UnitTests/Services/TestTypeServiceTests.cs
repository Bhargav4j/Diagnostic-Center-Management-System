using DiagnosticCenter.Application.Services;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DiagnosticCenter.UnitTests.Services;

public class TestTypeServiceTests
{
    private readonly Mock<ITestTypeRepository> _mockRepository;
    private readonly Mock<ILogger<TestTypeService>> _mockLogger;
    private readonly TestTypeService _service;

    public TestTypeServiceTests()
    {
        _mockRepository = new Mock<ITestTypeRepository>();
        _mockLogger = new Mock<ILogger<TestTypeService>>();
        _service = new TestTypeService(_mockRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllTestTypes()
    {
        var testTypes = new List<TestType>
        {
            new TestType { Id = 1, Name = "Blood Test", IsActive = true },
            new TestType { Id = 2, Name = "X-Ray", IsActive = true }
        };

        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(testTypes);

        var result = await _service.GetAllAsync();

        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(testTypes);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnTestType_WhenExists()
    {
        var testType = new TestType { Id = 1, Name = "Blood Test", IsActive = true };

        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(testType);

        var result = await _service.GetByIdAsync(1);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(testType);
    }
}
