using Xunit;
using Moq;
using DiagnosticCenter.Application.Services;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Tests.DiagnosticCenter.Application.Services;

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
    public async Task GetAllAsync_ShouldReturnAllTestSetups()
    {
        // Arrange
        var testSetups = new List<TestSetup>
        {
            new TestSetup { Id = 1, Name = "CBC Test", Fee = 50m },
            new TestSetup { Id = 2, Name = "Glucose Test", Fee = 30m }
        };
        _mockRepository.Setup(r => r.GetAllAsync(default)).ReturnsAsync(testSetups);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockRepository.Verify(r => r.GetAllAsync(default), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnTestSetup_WhenExists()
    {
        // Arrange
        var testSetup = new TestSetup { Id = 1, Name = "CBC Test", Fee = 50m };
        _mockRepository.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(testSetup);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("CBC Test", result.Name);
        _mockRepository.Verify(r => r.GetByIdAsync(1, default), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(999, default)).ReturnsAsync((TestSetup?)null);

        // Act
        var result = await _service.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
        _mockRepository.Verify(r => r.GetByIdAsync(999, default), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateTestSetup()
    {
        // Arrange
        var testSetup = new TestSetup { Name = "New Test", Fee = 75m, CreatedBy = "Admin" };
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<TestSetup>(), default))
            .ReturnsAsync((TestSetup t, CancellationToken ct) => { t.Id = 1; return t; });

        // Act
        var result = await _service.CreateAsync(testSetup);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.True(result.IsActive);
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<TestSetup>(), default), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateTestSetup_WhenExists()
    {
        // Arrange
        var existingTestSetup = new TestSetup { Id = 1, Name = "Old Test", Fee = 50m };
        var updatedTestSetup = new TestSetup { Name = "Updated Test", Fee = 100m, ModifiedBy = "Admin" };
        _mockRepository.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(existingTestSetup);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<TestSetup>(), default)).Returns(Task.CompletedTask);

        // Act
        await _service.UpdateAsync(1, updatedTestSetup);

        // Assert
        Assert.Equal("Updated Test", existingTestSetup.Name);
        Assert.Equal(100m, existingTestSetup.Fee);
        Assert.Equal("Admin", existingTestSetup.ModifiedBy);
        Assert.NotNull(existingTestSetup.ModifiedDate);
        _mockRepository.Verify(r => r.GetByIdAsync(1, default), Times.Once);
        _mockRepository.Verify(r => r.UpdateAsync(existingTestSetup, default), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowEntityNotFoundException_WhenNotExists()
    {
        // Arrange
        var updatedTestSetup = new TestSetup { Name = "Updated Test" };
        _mockRepository.Setup(r => r.GetByIdAsync(999, default)).ReturnsAsync((TestSetup?)null);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => _service.UpdateAsync(999, updatedTestSetup));
        _mockRepository.Verify(r => r.GetByIdAsync(999, default), Times.Once);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<TestSetup>(), default), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteTestSetup_WhenExists()
    {
        // Arrange
        _mockRepository.Setup(r => r.ExistsAsync(1, default)).ReturnsAsync(true);
        _mockRepository.Setup(r => r.DeleteAsync(1, default)).Returns(Task.CompletedTask);

        // Act
        await _service.DeleteAsync(1);

        // Assert
        _mockRepository.Verify(r => r.ExistsAsync(1, default), Times.Once);
        _mockRepository.Verify(r => r.DeleteAsync(1, default), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrowEntityNotFoundException_WhenNotExists()
    {
        // Arrange
        _mockRepository.Setup(r => r.ExistsAsync(999, default)).ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => _service.DeleteAsync(999));
        _mockRepository.Verify(r => r.ExistsAsync(999, default), Times.Once);
        _mockRepository.Verify(r => r.DeleteAsync(999, default), Times.Never);
    }

    [Fact]
    public async Task GetByTestTypeIdAsync_ShouldReturnTestSetups()
    {
        // Arrange
        var testTypeId = 1;
        var testSetups = new List<TestSetup>
        {
            new TestSetup { Id = 1, Name = "Test 1", TestTypeId = testTypeId },
            new TestSetup { Id = 2, Name = "Test 2", TestTypeId = testTypeId }
        };
        _mockRepository.Setup(r => r.GetByTestTypeIdAsync(testTypeId, default)).ReturnsAsync(testSetups);

        // Act
        var result = await _service.GetByTestTypeIdAsync(testTypeId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockRepository.Verify(r => r.GetByTestTypeIdAsync(testTypeId, default), Times.Once);
    }

    [Fact]
    public async Task SearchAsync_ShouldReturnMatchingTestSetups()
    {
        // Arrange
        var searchTerm = "CBC";
        var testSetups = new List<TestSetup>
        {
            new TestSetup { Id = 1, Name = "CBC Test" },
            new TestSetup { Id = 2, Name = "Complete CBC" }
        };
        _mockRepository.Setup(r => r.SearchAsync(searchTerm, default)).ReturnsAsync(testSetups);

        // Act
        var result = await _service.SearchAsync(searchTerm);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockRepository.Verify(r => r.SearchAsync(searchTerm, default), Times.Once);
    }

    [Fact]
    public async Task SearchAsync_ShouldReturnEmptyList_WhenNoMatches()
    {
        // Arrange
        var searchTerm = "NonExistent";
        _mockRepository.Setup(r => r.SearchAsync(searchTerm, default)).ReturnsAsync(new List<TestSetup>());

        // Act
        var result = await _service.SearchAsync(searchTerm);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        _mockRepository.Verify(r => r.SearchAsync(searchTerm, default), Times.Once);
    }
}
