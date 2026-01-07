using Xunit;
using Moq;
using DiagnosticCenter.Application.Services;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using DiagnosticCenter.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Tests.DiagnosticCenter.Application.Services;

public class PaymentServiceTests
{
    private readonly Mock<IPaymentRepository> _mockRepository;
    private readonly Mock<ITestEntryRepository> _mockTestEntryRepository;
    private readonly Mock<ILogger<PaymentService>> _mockLogger;
    private readonly PaymentService _service;

    public PaymentServiceTests()
    {
        _mockRepository = new Mock<IPaymentRepository>();
        _mockTestEntryRepository = new Mock<ITestEntryRepository>();
        _mockLogger = new Mock<ILogger<PaymentService>>();
        _service = new PaymentService(_mockRepository.Object, _mockTestEntryRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllPayments()
    {
        // Arrange
        var payments = new List<Payment>
        {
            new Payment { Id = 1, Amount = 100m, BillNumber = "BILL-001" },
            new Payment { Id = 2, Amount = 200m, BillNumber = "BILL-002" }
        };
        _mockRepository.Setup(r => r.GetAllAsync(default)).ReturnsAsync(payments);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockRepository.Verify(r => r.GetAllAsync(default), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnPayment_WhenExists()
    {
        // Arrange
        var payment = new Payment { Id = 1, Amount = 100m };
        _mockRepository.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(payment);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal(100m, result.Amount);
        _mockRepository.Verify(r => r.GetByIdAsync(1, default), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(999, default)).ReturnsAsync((Payment?)null);

        // Act
        var result = await _service.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
        _mockRepository.Verify(r => r.GetByIdAsync(999, default), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreatePayment_AndUpdateTestEntry()
    {
        // Arrange
        var testEntry = new TestEntry { Id = 1, TotalAmount = 500m, PaidAmount = 0m };
        var payment = new Payment { TestEntryId = 1, Amount = 200m, BillNumber = "BILL-001", CreatedBy = "Admin" };

        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Payment>(), default))
            .ReturnsAsync((Payment p, CancellationToken ct) => { p.Id = 1; return p; });
        _mockTestEntryRepository.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(testEntry);
        _mockTestEntryRepository.Setup(r => r.UpdateAsync(It.IsAny<TestEntry>(), default)).Returns(Task.CompletedTask);

        // Act
        var result = await _service.CreateAsync(payment);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.True(result.IsActive);
        Assert.Equal(200m, testEntry.PaidAmount);
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Payment>(), default), Times.Once);
        _mockTestEntryRepository.Verify(r => r.GetByIdAsync(1, default), Times.Once);
        _mockTestEntryRepository.Verify(r => r.UpdateAsync(testEntry, default), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreatePayment_WhenTestEntryNotFound()
    {
        // Arrange
        var payment = new Payment { TestEntryId = 999, Amount = 200m, BillNumber = "BILL-001" };

        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Payment>(), default))
            .ReturnsAsync((Payment p, CancellationToken ct) => { p.Id = 1; return p; });
        _mockTestEntryRepository.Setup(r => r.GetByIdAsync(999, default)).ReturnsAsync((TestEntry?)null);

        // Act
        var result = await _service.CreateAsync(payment);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Payment>(), default), Times.Once);
        _mockTestEntryRepository.Verify(r => r.GetByIdAsync(999, default), Times.Once);
        _mockTestEntryRepository.Verify(r => r.UpdateAsync(It.IsAny<TestEntry>(), default), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdatePayment_WhenExists()
    {
        // Arrange
        var existingPayment = new Payment { Id = 1, Amount = 100m, PaymentMethod = "Cash" };
        var updatedPayment = new Payment { Amount = 150m, PaymentMethod = "Card", ModifiedBy = "Admin" };
        _mockRepository.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(existingPayment);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Payment>(), default)).Returns(Task.CompletedTask);

        // Act
        await _service.UpdateAsync(1, updatedPayment);

        // Assert
        Assert.Equal(150m, existingPayment.Amount);
        Assert.Equal("Card", existingPayment.PaymentMethod);
        Assert.Equal("Admin", existingPayment.ModifiedBy);
        Assert.NotNull(existingPayment.ModifiedDate);
        _mockRepository.Verify(r => r.GetByIdAsync(1, default), Times.Once);
        _mockRepository.Verify(r => r.UpdateAsync(existingPayment, default), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowEntityNotFoundException_WhenNotExists()
    {
        // Arrange
        var updatedPayment = new Payment { Amount = 150m };
        _mockRepository.Setup(r => r.GetByIdAsync(999, default)).ReturnsAsync((Payment?)null);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => _service.UpdateAsync(999, updatedPayment));
        _mockRepository.Verify(r => r.GetByIdAsync(999, default), Times.Once);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Payment>(), default), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeletePayment_WhenExists()
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
    public async Task GetByTestEntryIdAsync_ShouldReturnPayments()
    {
        // Arrange
        var testEntryId = 1;
        var payments = new List<Payment>
        {
            new Payment { Id = 1, TestEntryId = testEntryId, Amount = 100m },
            new Payment { Id = 2, TestEntryId = testEntryId, Amount = 200m }
        };
        _mockRepository.Setup(r => r.GetByTestEntryIdAsync(testEntryId, default)).ReturnsAsync(payments);

        // Act
        var result = await _service.GetByTestEntryIdAsync(testEntryId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockRepository.Verify(r => r.GetByTestEntryIdAsync(testEntryId, default), Times.Once);
    }

    [Fact]
    public async Task SearchAsync_ShouldReturnMatchingPayments()
    {
        // Arrange
        var searchTerm = "BILL-001";
        var payments = new List<Payment>
        {
            new Payment { Id = 1, BillNumber = "BILL-001", Amount = 100m },
            new Payment { Id = 2, BillNumber = "BILL-001-A", Amount = 200m }
        };
        _mockRepository.Setup(r => r.SearchAsync(searchTerm, default)).ReturnsAsync(payments);

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
        _mockRepository.Setup(r => r.SearchAsync(searchTerm, default)).ReturnsAsync(new List<Payment>());

        // Act
        var result = await _service.SearchAsync(searchTerm);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        _mockRepository.Verify(r => r.SearchAsync(searchTerm, default), Times.Once);
    }
}
