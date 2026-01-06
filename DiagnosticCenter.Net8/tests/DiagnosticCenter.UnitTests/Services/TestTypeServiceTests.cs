using DiagnosticCenter.Application.Services;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Exceptions;
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
        // Arrange
        var testTypes = new List<TestType>
        {
            new TestType { Id = 1, Name = "Blood Test", IsActive = true },
            new TestType { Id = 2, Name = "X-Ray", IsActive = true }
        };
        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(testTypes);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(testTypes);
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ReturnsTestType()
    {
        // Arrange
        var testType = new TestType { Id = 1, Name = "Blood Test", IsActive = true };
        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(testType);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Name.Should().Be("Blood Test");
    }

    [Fact]
    public async Task CreateAsync_WithUniqueName_CreatesTestType()
    {
        // Arrange
        var testType = new TestType { Name = "Blood Test", CreatedBy = "admin" };
        _mockRepository.Setup(r => r.NameExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<TestType>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(testType);

        // Act
        var result = await _service.CreateAsync(testType);

        // Assert
        result.Should().NotBeNull();
        result.IsActive.Should().BeTrue();
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<TestType>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithDuplicateName_ThrowsDuplicateEntityException()
    {
        // Arrange
        var testType = new TestType { Name = "Blood Test", CreatedBy = "admin" };
        _mockRepository.Setup(r => r.NameExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<DuplicateEntityException>(() => _service.CreateAsync(testType));
    }

    [Fact]
    public async Task DeleteAsync_WithNonExistentId_ThrowsEntityNotFoundException()
    {
        // Arrange
        _mockRepository.Setup(r => r.ExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => _service.DeleteAsync(999));
    }
}
