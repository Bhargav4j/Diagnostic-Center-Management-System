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
    public async Task GetAllAsync_ReturnsAllTestTypes()
    {
        var testTypes = new List<TestType>
        {
            new TestType { Id = 1, Name = "Blood Test", IsActive = true },
            new TestType { Id = 2, Name = "X-Ray", IsActive = true }
        };

        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(testTypes);

        var result = await _service.GetAllAsync();

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(testTypes);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsTestType_WhenExists()
    {
        var testType = new TestType { Id = 1, Name = "Blood Test", IsActive = true };

        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(testType);

        var result = await _service.GetByIdAsync(1);

        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Name.Should().Be("Blood Test");
    }

    [Fact]
    public async Task CreateAsync_CreatesNewTestType()
    {
        var testType = new TestType { Name = "New Test", CreatedBy = "Admin" };

        _mockRepository.Setup(r => r.AddAsync(It.IsAny<TestType>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(testType);

        var result = await _service.CreateAsync(testType);

        result.Should().NotBeNull();
        result.IsActive.Should().BeTrue();
        result.CreatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<TestType>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
