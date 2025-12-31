using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Application.Services;

namespace DiagnosticCenter.UnitTests.Services;

public class TestTypeServiceTests
{
    private readonly Mock<ITestTypeRepository> _repositoryMock;
    private readonly Mock<ILogger<TestTypeService>> _loggerMock;
    private readonly TestTypeService _service;

    public TestTypeServiceTests()
    {
        _repositoryMock = new Mock<ITestTypeRepository>();
        _loggerMock = new Mock<ILogger<TestTypeService>>();
        _service = new TestTypeService(_repositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllTestTypes()
    {
        // Arrange
        var testTypes = new List<TestType>
        {
            new TestType { Id = 1, Name = "Blood Test", IsActive = true },
            new TestType { Id = 2, Name = "X-Ray", IsActive = true }
        };
        _repositoryMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(testTypes);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(testTypes);
    }

    [Fact]
    public async Task CreateAsync_WithValidData_ShouldCreateTestType()
    {
        // Arrange
        var testType = new TestType { Name = "New Test", CreatedBy = "Admin" };
        _repositoryMock.Setup(r => r.ExistsByNameAsync(testType.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<TestType>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(testType);

        // Act
        var result = await _service.CreateAsync(testType);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("New Test");
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<TestType>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
