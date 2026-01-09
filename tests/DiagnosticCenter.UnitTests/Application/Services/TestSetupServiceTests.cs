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

public class TestSetupServiceTests
{
    private readonly Mock<ITestSetupRepository> _mockRepository;
    private readonly Mock<ILogger<TestSetupService>> _mockLogger;
    private readonly TestSetupService _service;

    public TestSetupServiceTests()
    {
        _mockRepository = new Mock<ITestSetupRepository>();
        _mockLogger = new Mock<ILogger<TestSetupService>>();
        _service = new TestSetupService(_mockRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenRepositoryIsNull()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() => new TestSetupService(null!, _mockLogger.Object));
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() => new TestSetupService(_mockRepository.Object, null!));
    }

    [Fact]
    public void Constructor_ShouldCreateInstance_WhenValidParametersProvided()
    {
        // Arrange & Act
        var service = new TestSetupService(_mockRepository.Object, _mockLogger.Object);

        // Assert
        Assert.NotNull(service);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllTestSetups()
    {
        // Arrange
        var testSetups = new List<TestSetup>
        {
            new TestSetup { Id = 1, Name = "CBC Test", Fee = 100m },
            new TestSetup { Id = 2, Name = "Lipid Profile", Fee = 200m }
        };
        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(testSetups);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockRepository.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoTestSetupsExist()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<TestSetup>());

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
    public async Task GetByIdAsync_ShouldReturnTestSetup_WhenValidIdProvided()
    {
        // Arrange
        var testSetup = new TestSetup { Id = 1, Name = "CBC Test", Fee = 100m };
        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(testSetup);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("CBC Test", result.Name);
        _mockRepository.Verify(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenTestSetupNotFound()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TestSetup?)null);

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
    public async Task GetByTestTypeIdAsync_ShouldReturnMatchingTestSetups()
    {
        // Arrange
        var testSetups = new List<TestSetup>
        {
            new TestSetup { Id = 1, Name = "CBC Test", TestTypeId = 10 },
            new TestSetup { Id = 2, Name = "Glucose Test", TestTypeId = 10 }
        };
        _mockRepository.Setup(r => r.GetByTestTypeIdAsync(10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(testSetups);

        // Act
        var result = await _service.GetByTestTypeIdAsync(10);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockRepository.Verify(r => r.GetByTestTypeIdAsync(10, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByTestTypeIdAsync_ShouldReturnEmptyList_WhenNoMatches()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByTestTypeIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<TestSetup>());

        // Act
        var result = await _service.GetByTestTypeIdAsync(999);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetByTestTypeIdAsync_ShouldThrowException_WhenRepositoryThrows()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByTestTypeIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.GetByTestTypeIdAsync(10));
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateTestSetup_WithCorrectProperties()
    {
        // Arrange
        var testSetup = new TestSetup { Name = "Kidney Function", Fee = 300m, CreatedBy = "Admin" };
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<TestSetup>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((TestSetup t, CancellationToken ct) => { t.Id = 1; return t; });

        // Act
        var result = await _service.CreateAsync(testSetup);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Kidney Function", result.Name);
        Assert.True(result.IsActive);
        Assert.NotEqual(default(DateTime), result.CreatedDate);
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<TestSetup>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldSetCreatedDateToUtcNow()
    {
        // Arrange
        var testSetup = new TestSetup { Name = "Liver Function", Fee = 250m };
        var beforeCreate = DateTime.UtcNow;
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<TestSetup>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((TestSetup t, CancellationToken ct) => t);

        // Act
        var result = await _service.CreateAsync(testSetup);
        var afterCreate = DateTime.UtcNow;

        // Assert
        Assert.True(result.CreatedDate >= beforeCreate && result.CreatedDate <= afterCreate);
    }

    [Fact]
    public async Task CreateAsync_ShouldSetIsActiveToTrue()
    {
        // Arrange
        var testSetup = new TestSetup { Name = "ECG", Fee = 150m, IsActive = false };
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<TestSetup>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((TestSetup t, CancellationToken ct) => t);

        // Act
        var result = await _service.CreateAsync(testSetup);

        // Assert
        Assert.True(result.IsActive);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenRepositoryThrows()
    {
        // Arrange
        var testSetup = new TestSetup { Name = "Test", Fee = 100m };
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<TestSetup>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.CreateAsync(testSetup));
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateTestSetup()
    {
        // Arrange
        var testSetup = new TestSetup { Id = 1, Name = "Updated Test", Fee = 200m };
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<TestSetup>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.UpdateAsync(testSetup);

        // Assert
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<TestSetup>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldSetModifiedDateToUtcNow()
    {
        // Arrange
        var testSetup = new TestSetup { Id = 1, Name = "Test", Fee = 100m };
        var beforeUpdate = DateTime.UtcNow;
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<TestSetup>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.UpdateAsync(testSetup);
        var afterUpdate = DateTime.UtcNow;

        // Assert
        Assert.NotNull(testSetup.ModifiedDate);
        Assert.True(testSetup.ModifiedDate >= beforeUpdate && testSetup.ModifiedDate <= afterUpdate);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowException_WhenRepositoryThrows()
    {
        // Arrange
        var testSetup = new TestSetup { Id = 1, Name = "Test", Fee = 100m };
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<TestSetup>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(testSetup));
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteTestSetup_WhenValidIdProvided()
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
    public async Task SearchAsync_ShouldReturnMatchingTestSetups()
    {
        // Arrange
        var testSetups = new List<TestSetup>
        {
            new TestSetup { Id = 1, Name = "Blood Glucose" },
            new TestSetup { Id = 2, Name = "Blood Pressure" }
        };
        _mockRepository.Setup(r => r.SearchAsync("Blood", It.IsAny<CancellationToken>()))
            .ReturnsAsync(testSetups);

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
            .ReturnsAsync(new List<TestSetup>());

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
            .ReturnsAsync(new List<TestSetup>());

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
            .ReturnsAsync(new TestSetup());

        // Act
        await _service.GetByIdAsync(1, cts.Token);

        // Assert
        _mockRepository.Verify(r => r.GetByIdAsync(1, cts.Token), Times.Once);
    }

    [Fact]
    public async Task GetByTestTypeIdAsync_ShouldSupportCancellationToken()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        _mockRepository.Setup(r => r.GetByTestTypeIdAsync(10, cts.Token))
            .ReturnsAsync(new List<TestSetup>());

        // Act
        await _service.GetByTestTypeIdAsync(10, cts.Token);

        // Assert
        _mockRepository.Verify(r => r.GetByTestTypeIdAsync(10, cts.Token), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldSupportCancellationToken()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        var testSetup = new TestSetup { Name = "Test", Fee = 100m };
        _mockRepository.Setup(r => r.AddAsync(testSetup, cts.Token))
            .ReturnsAsync(testSetup);

        // Act
        await _service.CreateAsync(testSetup, cts.Token);

        // Assert
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<TestSetup>(), cts.Token), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldSupportCancellationToken()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        var testSetup = new TestSetup { Id = 1, Name = "Test", Fee = 100m };
        _mockRepository.Setup(r => r.UpdateAsync(testSetup, cts.Token))
            .Returns(Task.CompletedTask);

        // Act
        await _service.UpdateAsync(testSetup, cts.Token);

        // Assert
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<TestSetup>(), cts.Token), Times.Once);
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
            .ReturnsAsync(new List<TestSetup>());

        // Act
        await _service.SearchAsync("Test", cts.Token);

        // Assert
        _mockRepository.Verify(r => r.SearchAsync("Test", cts.Token), Times.Once);
    }
}
