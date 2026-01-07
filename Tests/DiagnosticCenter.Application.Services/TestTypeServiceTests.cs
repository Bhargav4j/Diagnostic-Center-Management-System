using Xunit;
using Moq;
using DiagnosticCenter.Application.Services;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiagnosticCenter.Application.Services.Tests;

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
    public void Constructor_WithNullRepository_ShouldThrowArgumentNullException()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() => new TestTypeService(null!, _mockLogger.Object));
    }

    [Fact]
    public void Constructor_WithNullLogger_ShouldThrowArgumentNullException()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() => new TestTypeService(_mockRepository.Object, null!));
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllTestTypes()
    {
        // Arrange
        var testTypes = new List<TestType>
        {
            new TestType { Id = 1, Name = "Blood Test" },
            new TestType { Id = 2, Name = "X-Ray" }
        };
        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(testTypes);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockRepository.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_WhenRepositoryThrows_ShouldThrowException()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.GetAllAsync());
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ShouldReturnTestType()
    {
        // Arrange
        var testType = new TestType { Id = 1, Name = "Blood Test" };
        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(testType);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Blood Test", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TestType?)null);

        // Act
        var result = await _service.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_WithValidEntity_ShouldCreateTestType()
    {
        // Arrange
        var testType = new TestType { Name = "New Test", CreatedBy = "Admin" };
        _mockRepository.Setup(r => r.ExistsByNameAsync(testType.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<TestType>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(testType);

        // Act
        var result = await _service.CreateAsync(testType);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsActive);
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<TestType>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithDuplicateName_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var testType = new TestType { Name = "Existing Test" };
        _mockRepository.Setup(r => r.ExistsByNameAsync(testType.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAsync(testType));
    }

    [Fact]
    public async Task UpdateAsync_WithValidId_ShouldUpdateTestType()
    {
        // Arrange
        var existingTestType = new TestType { Id = 1, Name = "Old Name" };
        var updatedTestType = new TestType { Name = "New Name", Description = "Updated" };
        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingTestType);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<TestType>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.UpdateAsync(1, updatedTestType);

        // Assert
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<TestType>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithInvalidId_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var updatedTestType = new TestType { Name = "New Name" };
        _mockRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TestType?)null);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.UpdateAsync(999, updatedTestType));
    }

    [Fact]
    public async Task DeleteAsync_WithValidId_ShouldDeleteTestType()
    {
        // Arrange
        _mockRepository.Setup(r => r.DeleteAsync(1, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.DeleteAsync(1);

        // Assert
        _mockRepository.Verify(r => r.DeleteAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SearchAsync_WithSearchTerm_ShouldReturnMatchingTestTypes()
    {
        // Arrange
        var searchTerm = "Blood";
        var testTypes = new List<TestType>
        {
            new TestType { Id = 1, Name = "Blood Test" },
            new TestType { Id = 2, Name = "Blood Sugar Test" }
        };
        _mockRepository.Setup(r => r.SearchAsync(searchTerm, It.IsAny<CancellationToken>()))
            .ReturnsAsync(testTypes);

        // Act
        var result = await _service.SearchAsync(searchTerm);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task ExistsByNameAsync_WhenExists_ShouldReturnTrue()
    {
        // Arrange
        _mockRepository.Setup(r => r.ExistsByNameAsync("Blood Test", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _service.ExistsByNameAsync("Blood Test");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ExistsByNameAsync_WhenNotExists_ShouldReturnFalse()
    {
        // Arrange
        _mockRepository.Setup(r => r.ExistsByNameAsync("NonExistent", It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _service.ExistsByNameAsync("NonExistent");

        // Assert
        Assert.False(result);
    }
}
