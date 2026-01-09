using DiagnosticCenter.Application.Services;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiagnosticCenter.UnitTests.Application.Services;

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
    public void Constructor_ShouldThrowArgumentNullException_WhenRepositoryIsNull()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() => new TestTypeService(null!, _mockLogger.Object));
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() => new TestTypeService(_mockRepository.Object, null!));
    }

    [Fact]
    public void Constructor_ShouldCreateInstance_WhenValidParametersProvided()
    {
        // Arrange & Act
        var service = new TestTypeService(_mockRepository.Object, _mockLogger.Object);

        // Assert
        Assert.NotNull(service);
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
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoTestTypesExist()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<TestType>());

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllAsync_ShouldThrowException_WhenRepositoryThrows()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.GetAllAsync());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnTestType_WhenValidIdProvided()
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
        _mockRepository.Verify(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenTestTypeNotFound()
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
    public async Task GetByIdAsync_ShouldThrowException_WhenRepositoryThrows()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.GetByIdAsync(1));
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateTestType_WithCorrectProperties()
    {
        // Arrange
        var testType = new TestType { Name = "MRI Scan", CreatedBy = "Admin" };
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<TestType>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((TestType t, CancellationToken ct) => { t.Id = 1; return t; });

        // Act
        var result = await _service.CreateAsync(testType);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("MRI Scan", result.Name);
        Assert.True(result.IsActive);
        Assert.NotEqual(default(DateTime), result.CreatedDate);
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<TestType>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldSetCreatedDateToUtcNow()
    {
        // Arrange
        var testType = new TestType { Name = "CT Scan" };
        var beforeCreate = DateTime.UtcNow;
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<TestType>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((TestType t, CancellationToken ct) => t);

        // Act
        var result = await _service.CreateAsync(testType);
        var afterCreate = DateTime.UtcNow;

        // Assert
        Assert.True(result.CreatedDate >= beforeCreate && result.CreatedDate <= afterCreate);
    }

    [Fact]
    public async Task CreateAsync_ShouldSetIsActiveToTrue()
    {
        // Arrange
        var testType = new TestType { Name = "Ultrasound", IsActive = false };
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<TestType>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((TestType t, CancellationToken ct) => t);

        // Act
        var result = await _service.CreateAsync(testType);

        // Assert
        Assert.True(result.IsActive);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenRepositoryThrows()
    {
        // Arrange
        var testType = new TestType { Name = "Test" };
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<TestType>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.CreateAsync(testType));
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateTestType()
    {
        // Arrange
        var testType = new TestType { Id = 1, Name = "Updated Test" };
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<TestType>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.UpdateAsync(testType);

        // Assert
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<TestType>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldSetModifiedDateToUtcNow()
    {
        // Arrange
        var testType = new TestType { Id = 1, Name = "Test" };
        var beforeUpdate = DateTime.UtcNow;
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<TestType>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.UpdateAsync(testType);
        var afterUpdate = DateTime.UtcNow;

        // Assert
        Assert.NotNull(testType.ModifiedDate);
        Assert.True(testType.ModifiedDate >= beforeUpdate && testType.ModifiedDate <= afterUpdate);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowException_WhenRepositoryThrows()
    {
        // Arrange
        var testType = new TestType { Id = 1, Name = "Test" };
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<TestType>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(testType));
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteTestType_WhenValidIdProvided()
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
    public async Task DeleteAsync_ShouldThrowException_WhenRepositoryThrows()
    {
        // Arrange
        _mockRepository.Setup(r => r.DeleteAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.DeleteAsync(1));
    }

    [Fact]
    public async Task SearchAsync_ShouldReturnMatchingTestTypes()
    {
        // Arrange
        var testTypes = new List<TestType>
        {
            new TestType { Id = 1, Name = "Blood Test" },
            new TestType { Id = 2, Name = "Blood Sugar" }
        };
        _mockRepository.Setup(r => r.SearchAsync("Blood", It.IsAny<CancellationToken>()))
            .ReturnsAsync(testTypes);

        // Act
        var result = await _service.SearchAsync("Blood");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockRepository.Verify(r => r.SearchAsync("Blood", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SearchAsync_ShouldReturnEmptyList_WhenNoMatches()
    {
        // Arrange
        _mockRepository.Setup(r => r.SearchAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<TestType>());

        // Act
        var result = await _service.SearchAsync("NonExistent");

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task SearchAsync_ShouldThrowException_WhenRepositoryThrows()
    {
        // Arrange
        _mockRepository.Setup(r => r.SearchAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.SearchAsync("Test"));
    }

    [Fact]
    public async Task GetAllAsync_ShouldSupportCancellationToken()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        _mockRepository.Setup(r => r.GetAllAsync(cts.Token))
            .ReturnsAsync(new List<TestType>());

        // Act
        await _service.GetAllAsync(cts.Token);

        // Assert
        _mockRepository.Verify(r => r.GetAllAsync(cts.Token), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldSupportCancellationToken()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        _mockRepository.Setup(r => r.GetByIdAsync(1, cts.Token))
            .ReturnsAsync(new TestType());

        // Act
        await _service.GetByIdAsync(1, cts.Token);

        // Assert
        _mockRepository.Verify(r => r.GetByIdAsync(1, cts.Token), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldSupportCancellationToken()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        var testType = new TestType { Name = "Test" };
        _mockRepository.Setup(r => r.AddAsync(testType, cts.Token))
            .ReturnsAsync(testType);

        // Act
        await _service.CreateAsync(testType, cts.Token);

        // Assert
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<TestType>(), cts.Token), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldSupportCancellationToken()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        var testType = new TestType { Id = 1, Name = "Test" };
        _mockRepository.Setup(r => r.UpdateAsync(testType, cts.Token))
            .Returns(Task.CompletedTask);

        // Act
        await _service.UpdateAsync(testType, cts.Token);

        // Assert
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<TestType>(), cts.Token), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldSupportCancellationToken()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        _mockRepository.Setup(r => r.DeleteAsync(1, cts.Token))
            .Returns(Task.CompletedTask);

        // Act
        await _service.DeleteAsync(1, cts.Token);

        // Assert
        _mockRepository.Verify(r => r.DeleteAsync(1, cts.Token), Times.Once);
    }

    [Fact]
    public async Task SearchAsync_ShouldSupportCancellationToken()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        _mockRepository.Setup(r => r.SearchAsync("Test", cts.Token))
            .ReturnsAsync(new List<TestType>());

        // Act
        await _service.SearchAsync("Test", cts.Token);

        // Assert
        _mockRepository.Verify(r => r.SearchAsync("Test", cts.Token), Times.Once);
    }
}
