using Xunit;
using Moq;
using DiagnosticCenter.Application.Services;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Tests.DiagnosticCenter.Application.Services;

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
    public async Task GetAllAsync_ShouldReturnAllTestTypes()
    {
        // Arrange
        var testTypes = new List<TestType>
        {
            new TestType { Id = 1, Name = "Blood Test" },
            new TestType { Id = 2, Name = "Urine Test" }
        };
        _mockRepository.Setup(r => r.GetAllAsync(default)).ReturnsAsync(testTypes);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockRepository.Verify(r => r.GetAllAsync(default), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnTestType_WhenExists()
    {
        // Arrange
        var testType = new TestType { Id = 1, Name = "Blood Test" };
        _mockRepository.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(testType);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Blood Test", result.Name);
        _mockRepository.Verify(r => r.GetByIdAsync(1, default), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(999, default)).ReturnsAsync((TestType?)null);

        // Act
        var result = await _service.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
        _mockRepository.Verify(r => r.GetByIdAsync(999, default), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateTestType_WhenNameDoesNotExist()
    {
        // Arrange
        var testType = new TestType { Name = "X-Ray Test", CreatedBy = "Admin" };
        _mockRepository.Setup(r => r.ExistsByNameAsync(testType.Name, default)).ReturnsAsync(false);
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<TestType>(), default))
            .ReturnsAsync((TestType t, CancellationToken ct) => { t.Id = 1; return t; });

        // Act
        var result = await _service.CreateAsync(testType);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.True(result.IsActive);
        _mockRepository.Verify(r => r.ExistsByNameAsync(testType.Name, default), Times.Once);
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<TestType>(), default), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowDuplicateEntityException_WhenNameExists()
    {
        // Arrange
        var testType = new TestType { Name = "Blood Test" };
        _mockRepository.Setup(r => r.ExistsByNameAsync(testType.Name, default)).ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<DuplicateEntityException>(() => _service.CreateAsync(testType));
        _mockRepository.Verify(r => r.ExistsByNameAsync(testType.Name, default), Times.Once);
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<TestType>(), default), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateTestType_WhenExists()
    {
        // Arrange
        var existingTestType = new TestType { Id = 1, Name = "Blood Test", Description = "Old" };
        var updatedTestType = new TestType { Name = "Blood Test Updated", Description = "New", ModifiedBy = "Admin" };
        _mockRepository.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(existingTestType);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<TestType>(), default)).Returns(Task.CompletedTask);

        // Act
        await _service.UpdateAsync(1, updatedTestType);

        // Assert
        Assert.Equal("Blood Test Updated", existingTestType.Name);
        Assert.Equal("New", existingTestType.Description);
        Assert.Equal("Admin", existingTestType.ModifiedBy);
        Assert.NotNull(existingTestType.ModifiedDate);
        _mockRepository.Verify(r => r.GetByIdAsync(1, default), Times.Once);
        _mockRepository.Verify(r => r.UpdateAsync(existingTestType, default), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowEntityNotFoundException_WhenNotExists()
    {
        // Arrange
        var updatedTestType = new TestType { Name = "Updated Test" };
        _mockRepository.Setup(r => r.GetByIdAsync(999, default)).ReturnsAsync((TestType?)null);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => _service.UpdateAsync(999, updatedTestType));
        _mockRepository.Verify(r => r.GetByIdAsync(999, default), Times.Once);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<TestType>(), default), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteTestType_WhenExists()
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
    public async Task SearchAsync_ShouldReturnMatchingTestTypes()
    {
        // Arrange
        var searchTerm = "Blood";
        var testTypes = new List<TestType>
        {
            new TestType { Id = 1, Name = "Blood Test" },
            new TestType { Id = 2, Name = "Complete Blood Count" }
        };
        _mockRepository.Setup(r => r.SearchAsync(searchTerm, default)).ReturnsAsync(testTypes);

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
        _mockRepository.Setup(r => r.SearchAsync(searchTerm, default)).ReturnsAsync(new List<TestType>());

        // Act
        var result = await _service.SearchAsync(searchTerm);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        _mockRepository.Verify(r => r.SearchAsync(searchTerm, default), Times.Once);
    }
}
