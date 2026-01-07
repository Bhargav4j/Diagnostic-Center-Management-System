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

public class TestSetupServiceTests
{
    private readonly Mock<ITestSetupRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<TestSetupService>> _mockLogger;
    private readonly TestSetupService _service;

    public TestSetupServiceTests()
    {
        _mockRepository = new Mock<ITestSetupRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<TestSetupService>>();
        _service = new TestSetupService(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
    }

    [Fact]
    public void Constructor_WithNullRepository_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new TestSetupService(null, _mockMapper.Object, _mockLogger.Object));
    }

    [Fact]
    public void Constructor_WithNullMapper_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new TestSetupService(_mockRepository.Object, null, _mockLogger.Object));
    }

    [Fact]
    public void Constructor_WithNullLogger_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new TestSetupService(_mockRepository.Object, _mockMapper.Object, null));
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllTestSetups()
    {
        var testSetups = new List<TestSetup> { new TestSetup { Id = 1, Name = "Setup1" } };
        var testSetupDtos = new List<TestSetupDto> { new TestSetupDto { Id = 1, Name = "Setup1" } };
        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(testSetups);
        _mockMapper.Setup(m => m.Map<IEnumerable<TestSetupDto>>(testSetups)).Returns(testSetupDtos);

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
    public async Task GetByIdAsync_WithValidId_ReturnsTestSetup()
    {
        var testSetup = new TestSetup { Id = 1, Name = "Setup1" };
        var testSetupDto = new TestSetupDto { Id = 1, Name = "Setup1" };
        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(testSetup);
        _mockMapper.Setup(m => m.Map<TestSetupDto>(testSetup)).Returns(testSetupDto);

        var result = await _service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ReturnsNull()
    {
        _mockRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync((TestSetup)null);

        var result = await _service.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_WithValidDto_ReturnsCreatedTestSetup()
    {
        var createDto = new TestSetupCreateDto { Name = "NewSetup" };
        var testSetup = new TestSetup { Id = 1, Name = "NewSetup" };
        var testSetupDto = new TestSetupDto { Id = 1, Name = "NewSetup" };
        _mockRepository.Setup(r => r.ExistsByNameAsync(createDto.Name, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _mockMapper.Setup(m => m.Map<TestSetup>(createDto)).Returns(testSetup);
        _mockRepository.Setup(r => r.AddAsync(testSetup, It.IsAny<CancellationToken>())).ReturnsAsync(testSetup);
        _mockMapper.Setup(m => m.Map<TestSetupDto>(testSetup)).Returns(testSetupDto);

        var result = await _service.CreateAsync(createDto);

        Assert.NotNull(result);
        Assert.Equal("NewSetup", result.Name);
    }

    [Fact]
    public async Task CreateAsync_WithDuplicateName_ThrowsInvalidOperationException()
    {
        var createDto = new TestSetupCreateDto { Name = "ExistingSetup" };
        _mockRepository.Setup(r => r.ExistsByNameAsync(createDto.Name, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAsync(createDto));
    }

    [Fact]
    public async Task CreateAsync_WithException_RethrowsException()
    {
        var createDto = new TestSetupCreateDto { Name = "NewSetup" };
        _mockRepository.Setup(r => r.ExistsByNameAsync(createDto.Name, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _mockMapper.Setup(m => m.Map<TestSetup>(createDto)).Throws(new Exception("Test exception"));

        await Assert.ThrowsAsync<Exception>(() => _service.CreateAsync(createDto));
    }

    [Fact]
    public async Task UpdateAsync_WithValidId_UpdatesTestSetup()
    {
        var updateDto = new TestSetupUpdateDto { Name = "UpdatedSetup" };
        var existing = new TestSetup { Id = 1, Name = "OldSetup" };
        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(existing);
        _mockMapper.Setup(m => m.Map(updateDto, existing));
        _mockRepository.Setup(r => r.UpdateAsync(existing, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        await _service.UpdateAsync(1, updateDto);

        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<TestSetup>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithInvalidId_ThrowsInvalidOperationException()
    {
        var updateDto = new TestSetupUpdateDto { Name = "UpdatedSetup" };
        _mockRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync((TestSetup)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.UpdateAsync(999, updateDto));
    }

    [Fact]
    public async Task DeleteAsync_WithValidId_DeletesTestSetup()
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
    public async Task SearchAsync_WithValidTerm_ReturnsMatchingTestSetups()
    {
        var searchTerm = "blood";
        var testSetups = new List<TestSetup> { new TestSetup { Id = 1, Name = "Blood Setup" } };
        var testSetupDtos = new List<TestSetupDto> { new TestSetupDto { Id = 1, Name = "Blood Setup" } };
        _mockRepository.Setup(r => r.SearchAsync(searchTerm, It.IsAny<CancellationToken>())).ReturnsAsync(testSetups);
        _mockMapper.Setup(m => m.Map<IEnumerable<TestSetupDto>>(testSetups)).Returns(testSetupDtos);

        var result = await _service.SearchAsync(searchTerm);

        Assert.NotNull(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task SearchAsync_WithEmptyTerm_ReturnsEmptyList()
    {
        var searchTerm = "";
        var testSetups = new List<TestSetup>();
        var testSetupDtos = new List<TestSetupDto>();
        _mockRepository.Setup(r => r.SearchAsync(searchTerm, It.IsAny<CancellationToken>())).ReturnsAsync(testSetups);
        _mockMapper.Setup(m => m.Map<IEnumerable<TestSetupDto>>(testSetups)).Returns(testSetupDtos);

        var result = await _service.SearchAsync(searchTerm);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task ExistsByNameAsync_WithExistingName_ReturnsTrue()
    {
        var name = "ExistingSetup";
        _mockRepository.Setup(r => r.ExistsByNameAsync(name, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var result = await _service.ExistsByNameAsync(name);

        Assert.True(result);
    }

    [Fact]
    public async Task ExistsByNameAsync_WithNonExistingName_ReturnsFalse()
    {
        var name = "NonExistingSetup";
        _mockRepository.Setup(r => r.ExistsByNameAsync(name, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var result = await _service.ExistsByNameAsync(name);

        Assert.False(result);
    }

    [Fact]
    public async Task ExistsByNameAsync_WithException_RethrowsException()
    {
        var name = "TestSetup";
        _mockRepository.Setup(r => r.ExistsByNameAsync(name, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Test exception"));

        await Assert.ThrowsAsync<Exception>(() => _service.ExistsByNameAsync(name));
    }
}
