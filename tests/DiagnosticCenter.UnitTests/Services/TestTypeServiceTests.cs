using AutoMapper;
using DiagnosticCenter.Application.DTOs;
using DiagnosticCenter.Application.Services;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DiagnosticCenter.UnitTests.Services;

/// <summary>
/// Unit tests for TestTypeService
/// </summary>
public class TestTypeServiceTests
{
    private readonly Mock<ITestTypeRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<TestTypeService>> _loggerMock;
    private readonly TestTypeService _service;

    public TestTypeServiceTests()
    {
        _repositoryMock = new Mock<ITestTypeRepository>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<TestTypeService>>();
        _service = new TestTypeService(_repositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllTestTypes()
    {
        // Arrange
        var entities = new List<TestType>
        {
            new TestType { Id = 1, Name = "Blood Test", IsActive = true },
            new TestType { Id = 2, Name = "X-Ray", IsActive = true }
        };
        var dtos = new List<TestTypeDto>
        {
            new TestTypeDto { Id = 1, Name = "Blood Test" },
            new TestTypeDto { Id = 2, Name = "X-Ray" }
        };

        _repositoryMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(entities);
        _mapperMock.Setup(m => m.Map<IEnumerable<TestTypeDto>>(entities))
            .Returns(dtos);

        // Act
        var result = await _service.GetAllAsync(CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.First().Name.Should().Be("Blood Test");
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ShouldReturnTestType()
    {
        // Arrange
        var entity = new TestType { Id = 1, Name = "Blood Test", IsActive = true };
        var dto = new TestTypeDto { Id = 1, Name = "Blood Test" };

        _repositoryMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(entity);
        _mapperMock.Setup(m => m.Map<TestTypeDto>(entity))
            .Returns(dto);

        // Act
        var result = await _service.GetByIdAsync(1, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Name.Should().Be("Blood Test");
    }

    [Fact]
    public async Task CreateAsync_WithValidDto_ShouldCreateTestType()
    {
        // Arrange
        var createDto = new TestTypeCreateDto { Name = "New Test", Description = "Test Description" };
        var entity = new TestType { Id = 1, Name = "New Test", IsActive = true };
        var resultDto = new TestTypeDto { Id = 1, Name = "New Test" };

        _mapperMock.Setup(m => m.Map<TestType>(createDto))
            .Returns(entity);
        _repositoryMock.Setup(r => r.AddAsync(entity, It.IsAny<CancellationToken>()))
            .ReturnsAsync(entity);
        _mapperMock.Setup(m => m.Map<TestTypeDto>(entity))
            .Returns(resultDto);

        // Act
        var result = await _service.CreateAsync(createDto, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("New Test");
    }

    [Fact]
    public async Task DeleteAsync_WithValidId_ShouldDeleteTestType()
    {
        // Arrange
        _repositoryMock.Setup(r => r.ExistsAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        await _service.DeleteAsync(1, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(r => r.DeleteAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }
}
