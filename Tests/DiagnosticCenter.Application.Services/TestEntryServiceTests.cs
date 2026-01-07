using Xunit;
using Moq;
using DiagnosticCenter.Application.Services;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Tests.DiagnosticCenter.Application.Services;

public class TestEntryServiceTests
{
    private readonly Mock<ITestEntryRepository> _mockRepository;
    private readonly Mock<ILogger<TestEntryService>> _mockLogger;
    private readonly TestEntryService _service;

    public TestEntryServiceTests()
    {
        _mockRepository = new Mock<ITestEntryRepository>();
        _mockLogger = new Mock<ILogger<TestEntryService>>();
        _service = new TestEntryService(_mockRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllTestEntries()
    {
        // Arrange
        var testEntries = new List<TestEntry>
        {
            new TestEntry { Id = 1, PatientName = "John Doe", TotalAmount = 500m },
            new TestEntry { Id = 2, PatientName = "Jane Smith", TotalAmount = 300m }
        };
        _mockRepository.Setup(r => r.GetAllAsync(default)).ReturnsAsync(testEntries);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockRepository.Verify(r => r.GetAllAsync(default), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnTestEntry_WhenExists()
    {
        // Arrange
        var testEntry = new TestEntry { Id = 1, PatientName = "John Doe" };
        _mockRepository.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(testEntry);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("John Doe", result.PatientName);
        _mockRepository.Verify(r => r.GetByIdAsync(1, default), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(999, default)).ReturnsAsync((TestEntry?)null);

        // Act
        var result = await _service.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
        _mockRepository.Verify(r => r.GetByIdAsync(999, default), Times.Once);
    }

    [Fact]
    public async Task GetByBillNumberAsync_ShouldReturnTestEntry_WhenExists()
    {
        // Arrange
        var billNumber = "BILL-2024010112345";
        var testEntry = new TestEntry { Id = 1, BillNumber = billNumber };
        _mockRepository.Setup(r => r.GetByBillNumberAsync(billNumber, default)).ReturnsAsync(testEntry);

        // Act
        var result = await _service.GetByBillNumberAsync(billNumber);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(billNumber, result.BillNumber);
        _mockRepository.Verify(r => r.GetByBillNumberAsync(billNumber, default), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateTestEntry_WithGeneratedBillNumber()
    {
        // Arrange
        var testEntry = new TestEntry { PatientName = "John Doe", TotalAmount = 500m, CreatedBy = "Admin" };
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<TestEntry>(), default))
            .ReturnsAsync((TestEntry t, CancellationToken ct) => { t.Id = 1; return t; });

        // Act
        var result = await _service.CreateAsync(testEntry);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.True(result.IsActive);
        Assert.NotEmpty(result.BillNumber);
        Assert.StartsWith("BILL-", result.BillNumber);
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<TestEntry>(), default), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateTestEntry_WhenExists()
    {
        // Arrange
        var existingEntry = new TestEntry { Id = 1, PatientName = "Old Name", TotalAmount = 100m };
        var updatedEntry = new TestEntry { PatientName = "New Name", TotalAmount = 200m, ModifiedBy = "Admin" };
        _mockRepository.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(existingEntry);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<TestEntry>(), default)).Returns(Task.CompletedTask);

        // Act
        await _service.UpdateAsync(1, updatedEntry);

        // Assert
        Assert.Equal("New Name", existingEntry.PatientName);
        Assert.Equal(200m, existingEntry.TotalAmount);
        Assert.Equal("Admin", existingEntry.ModifiedBy);
        Assert.NotNull(existingEntry.ModifiedDate);
        _mockRepository.Verify(r => r.GetByIdAsync(1, default), Times.Once);
        _mockRepository.Verify(r => r.UpdateAsync(existingEntry, default), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowEntityNotFoundException_WhenNotExists()
    {
        // Arrange
        var updatedEntry = new TestEntry { PatientName = "John Doe" };
        _mockRepository.Setup(r => r.GetByIdAsync(999, default)).ReturnsAsync((TestEntry?)null);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => _service.UpdateAsync(999, updatedEntry));
        _mockRepository.Verify(r => r.GetByIdAsync(999, default), Times.Once);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<TestEntry>(), default), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteTestEntry_WhenExists()
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
    public async Task SearchAsync_ShouldReturnMatchingTestEntries()
    {
        // Arrange
        var searchTerm = "John";
        var testEntries = new List<TestEntry>
        {
            new TestEntry { Id = 1, PatientName = "John Doe" },
            new TestEntry { Id = 2, PatientName = "Johnny Smith" }
        };
        _mockRepository.Setup(r => r.SearchAsync(searchTerm, default)).ReturnsAsync(testEntries);

        // Act
        var result = await _service.SearchAsync(searchTerm);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockRepository.Verify(r => r.SearchAsync(searchTerm, default), Times.Once);
    }

    [Fact]
    public async Task GetUnpaidTestsAsync_ShouldReturnUnpaidTests()
    {
        // Arrange
        var unpaidTests = new List<TestEntry>
        {
            new TestEntry { Id = 1, TotalAmount = 500m, PaidAmount = 200m },
            new TestEntry { Id = 2, TotalAmount = 300m, PaidAmount = 0m }
        };
        _mockRepository.Setup(r => r.GetUnpaidTestsAsync(default)).ReturnsAsync(unpaidTests);

        // Act
        var result = await _service.GetUnpaidTestsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockRepository.Verify(r => r.GetUnpaidTestsAsync(default), Times.Once);
    }

    [Fact]
    public async Task SearchAsync_ShouldReturnEmptyList_WhenNoMatches()
    {
        // Arrange
        var searchTerm = "NonExistent";
        _mockRepository.Setup(r => r.SearchAsync(searchTerm, default)).ReturnsAsync(new List<TestEntry>());

        // Act
        var result = await _service.SearchAsync(searchTerm);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        _mockRepository.Verify(r => r.SearchAsync(searchTerm, default), Times.Once);
    }
}
