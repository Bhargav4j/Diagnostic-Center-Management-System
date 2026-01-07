using Xunit;
using Moq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using DiagnosticCenter.Application.Services;
using DiagnosticCenter.Application.DTOs;
using DiagnosticCenter.Application.Interfaces;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Tests.DiagnosticCenter.Application;

public class TestTypeServiceTests
{
    private readonly Mock<ITestTypeRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<TestTypeService>> _mockLogger;
    private readonly TestTypeService _service;

    public TestTypeServiceTests()
    {
        _mockRepository = new Mock<ITestTypeRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<TestTypeService>>();
        _service = new TestTypeService(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
    }

    [Fact]
    public void Constructor_WithNullRepository_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new TestTypeService(null, _mockMapper.Object, _mockLogger.Object));
    }

    [Fact]
    public void Constructor_WithNullMapper_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new TestTypeService(_mockRepository.Object, null, _mockLogger.Object));
    }

    [Fact]
    public void Constructor_WithNullLogger_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new TestTypeService(_mockRepository.Object, _mockMapper.Object, null));
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllTestTypes()
    {
        var testTypes = new List<TestType> { new TestType { Id = 1, Name = "Test1" } };
        var testTypeDtos = new List<TestTypeDto> { new TestTypeDto { Id = 1, Name = "Test1" } };
        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(testTypes);
        _mockMapper.Setup(m => m.Map<IEnumerable<TestTypeDto>>(testTypes)).Returns(testTypeDtos);

        var result = await _service.GetAllAsync();

        Assert.NotNull(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetAllAsync_ThrowsException_RethrowsException()
    {
        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Test exception"));

        await Assert.ThrowsAsync<Exception>(() => _service.GetAllAsync());
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ReturnsTestType()
    {
        var testType = new TestType { Id = 1, Name = "Test1" };
        var testTypeDto = new TestTypeDto { Id = 1, Name = "Test1" };
        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(testType);
        _mockMapper.Setup(m => m.Map<TestTypeDto>(testType)).Returns(testTypeDto);

        var result = await _service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ReturnsNull()
    {
        _mockRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync((TestType)null);

        var result = await _service.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_WithValidDto_ReturnsCreatedTestType()
    {
        var createDto = new TestTypeCreateDto { Name = "NewTest" };
        var testType = new TestType { Id = 1, Name = "NewTest" };
        var testTypeDto = new TestTypeDto { Id = 1, Name = "NewTest" };
        _mockMapper.Setup(m => m.Map<TestType>(createDto)).Returns(testType);
        _mockRepository.Setup(r => r.AddAsync(testType, It.IsAny<CancellationToken>())).ReturnsAsync(testType);
        _mockMapper.Setup(m => m.Map<TestTypeDto>(testType)).Returns(testTypeDto);

        var result = await _service.CreateAsync(createDto);

        Assert.NotNull(result);
        Assert.Equal("NewTest", result.Name);
    }

    [Fact]
    public async Task CreateAsync_WithException_RethrowsException()
    {
        var createDto = new TestTypeCreateDto { Name = "NewTest" };
        _mockMapper.Setup(m => m.Map<TestType>(createDto)).Throws(new Exception("Test exception"));

        await Assert.ThrowsAsync<Exception>(() => _service.CreateAsync(createDto));
    }

    [Fact]
    public async Task UpdateAsync_WithValidId_UpdatesTestType()
    {
        var updateDto = new TestTypeUpdateDto { Name = "UpdatedTest" };
        var existing = new TestType { Id = 1, Name = "OldTest" };
        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(existing);
        _mockMapper.Setup(m => m.Map(updateDto, existing));
        _mockRepository.Setup(r => r.UpdateAsync(existing, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        await _service.UpdateAsync(1, updateDto);

        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<TestType>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithInvalidId_ThrowsInvalidOperationException()
    {
        var updateDto = new TestTypeUpdateDto { Name = "UpdatedTest" };
        _mockRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync((TestType)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.UpdateAsync(999, updateDto));
    }

    [Fact]
    public async Task DeleteAsync_WithValidId_DeletesTestType()
    {
        _mockRepository.Setup(r => r.DeleteAsync(1, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        await _service.DeleteAsync(1);

        _mockRepository.Verify(r => r.DeleteAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WithException_RethrowsException()
    {
        _mockRepository.Setup(r => r.DeleteAsync(1, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Test exception"));

        await Assert.ThrowsAsync<Exception>(() => _service.DeleteAsync(1));
    }

    [Fact]
    public async Task SearchAsync_WithValidTerm_ReturnsMatchingTestTypes()
    {
        var searchTerm = "blood";
        var testTypes = new List<TestType> { new TestType { Id = 1, Name = "Blood Test" } };
        var testTypeDtos = new List<TestTypeDto> { new TestTypeDto { Id = 1, Name = "Blood Test" } };
        _mockRepository.Setup(r => r.SearchAsync(searchTerm, It.IsAny<CancellationToken>())).ReturnsAsync(testTypes);
        _mockMapper.Setup(m => m.Map<IEnumerable<TestTypeDto>>(testTypes)).Returns(testTypeDtos);

        var result = await _service.SearchAsync(searchTerm);

        Assert.NotNull(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task SearchAsync_WithEmptyTerm_ReturnsEmptyList()
    {
        var searchTerm = "";
        var testTypes = new List<TestType>();
        var testTypeDtos = new List<TestTypeDto>();
        _mockRepository.Setup(r => r.SearchAsync(searchTerm, It.IsAny<CancellationToken>())).ReturnsAsync(testTypes);
        _mockMapper.Setup(m => m.Map<IEnumerable<TestTypeDto>>(testTypes)).Returns(testTypeDtos);

        var result = await _service.SearchAsync(searchTerm);

        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
