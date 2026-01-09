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

public class PaymentServiceTests
{
    private readonly Mock<IPaymentRepository> _mockRepository;
    private readonly Mock<IPatientTestRepository> _mockPatientTestRepository;
    private readonly Mock<ILogger<PaymentService>> _mockLogger;
    private readonly PaymentService _service;

    public PaymentServiceTests()
    {
        _mockRepository = new Mock<IPaymentRepository>();
        _mockPatientTestRepository = new Mock<IPatientTestRepository>();
        _mockLogger = new Mock<ILogger<PaymentService>>();
        _service = new PaymentService(_mockRepository.Object, _mockPatientTestRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenRepositoryIsNull()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new PaymentService(null!, _mockPatientTestRepository.Object, _mockLogger.Object));
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenPatientTestRepositoryIsNull()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new PaymentService(_mockRepository.Object, null!, _mockLogger.Object));
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new PaymentService(_mockRepository.Object, _mockPatientTestRepository.Object, null!));
    }

    [Fact]
    public void Constructor_ShouldCreateInstance_WhenValidParametersProvided()
    {
        // Arrange & Act
        var service = new PaymentService(_mockRepository.Object, _mockPatientTestRepository.Object, _mockLogger.Object);

        // Assert
        Assert.NotNull(service);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllPayments()
    {
        // Arrange
        var payments = new List<Payment>
        {
            new Payment { Id = 1, BillNo = "BILL001", AmountPaid = 500m },
            new Payment { Id = 2, BillNo = "BILL002", AmountPaid = 750m }
        };
        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(payments);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockRepository.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoPaymentsExist()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Payment>());

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
    public async Task GetByIdAsync_ShouldReturnPayment_WhenValidIdProvided()
    {
        // Arrange
        var payment = new Payment { Id = 1, BillNo = "BILL001", AmountPaid = 500m };
        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(payment);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("BILL001", result.BillNo);
        _mockRepository.Verify(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenPaymentNotFound()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Payment?)null);

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
    public async Task GetByBillNoAsync_ShouldReturnMatchingPayments()
    {
        // Arrange
        var payments = new List<Payment>
        {
            new Payment { Id = 1, BillNo = "BILL001", AmountPaid = 250m },
            new Payment { Id = 2, BillNo = "BILL001", AmountPaid = 250m }
        };
        _mockRepository.Setup(r => r.GetByBillNoAsync("BILL001", It.IsAny<CancellationToken>()))
            .ReturnsAsync(payments);

        // Act
        var result = await _service.GetByBillNoAsync("BILL001");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockRepository.Verify(r => r.GetByBillNoAsync("BILL001", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByBillNoAsync_ShouldReturnEmptyList_WhenNoMatches()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByBillNoAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Payment>());

        // Act
        var result = await _service.GetByBillNoAsync("NONEXISTENT");

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetByBillNoAsync_ShouldThrowException_WhenRepositoryThrows()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByBillNoAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.GetByBillNoAsync("BILL001"));
    }

    [Fact]
    public async Task CreateAsync_ShouldCreatePayment_WithCorrectProperties()
    {
        // Arrange
        var payment = new Payment { BillNo = "BILL001", AmountPaid = 500m, PatientTestId = 1 };
        var patientTest = new PatientTest { Id = 1, TotalAmount = 1000m, PaidAmount = 0m };

        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Payment>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Payment p, CancellationToken ct) => { p.Id = 1; return p; });
        _mockPatientTestRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(patientTest);
        _mockPatientTestRepository.Setup(r => r.UpdateAsync(It.IsAny<PatientTest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.CreateAsync(payment);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsActive);
        Assert.NotEqual(default(DateTime), result.CreatedDate);
        Assert.NotEqual(default(DateTime), result.PaymentDate);
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Payment>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldSetCreatedDateToUtcNow()
    {
        // Arrange
        var payment = new Payment { BillNo = "BILL001", AmountPaid = 500m, PatientTestId = 1 };
        var beforeCreate = DateTime.UtcNow;

        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Payment>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Payment p, CancellationToken ct) => p);
        _mockPatientTestRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((PatientTest?)null);

        // Act
        var result = await _service.CreateAsync(payment);
        var afterCreate = DateTime.UtcNow;

        // Assert
        Assert.True(result.CreatedDate >= beforeCreate && result.CreatedDate <= afterCreate);
    }

    [Fact]
    public async Task CreateAsync_ShouldSetPaymentDateToUtcNow()
    {
        // Arrange
        var payment = new Payment { BillNo = "BILL001", AmountPaid = 500m, PatientTestId = 1 };
        var beforeCreate = DateTime.UtcNow;

        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Payment>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Payment p, CancellationToken ct) => p);
        _mockPatientTestRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((PatientTest?)null);

        // Act
        var result = await _service.CreateAsync(payment);
        var afterCreate = DateTime.UtcNow;

        // Assert
        Assert.True(result.PaymentDate >= beforeCreate && result.PaymentDate <= afterCreate);
    }

    [Fact]
    public async Task CreateAsync_ShouldSetIsActiveToTrue()
    {
        // Arrange
        var payment = new Payment { BillNo = "BILL001", AmountPaid = 500m, PatientTestId = 1, IsActive = false };

        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Payment>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Payment p, CancellationToken ct) => p);
        _mockPatientTestRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((PatientTest?)null);

        // Act
        var result = await _service.CreateAsync(payment);

        // Assert
        Assert.True(result.IsActive);
    }

    [Fact]
    public async Task CreateAsync_ShouldUpdatePatientTestPaidAmount()
    {
        // Arrange
        var payment = new Payment { BillNo = "BILL001", AmountPaid = 500m, PatientTestId = 1 };
        var patientTest = new PatientTest { Id = 1, TotalAmount = 1000m, PaidAmount = 200m };

        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Payment>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Payment p, CancellationToken ct) => p);
        _mockPatientTestRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(patientTest);
        _mockPatientTestRepository.Setup(r => r.UpdateAsync(It.IsAny<PatientTest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.CreateAsync(payment);

        // Assert
        Assert.Equal(700m, patientTest.PaidAmount);
        _mockPatientTestRepository.Verify(r => r.UpdateAsync(It.IsAny<PatientTest>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldNotThrowException_WhenPatientTestNotFound()
    {
        // Arrange
        var payment = new Payment { BillNo = "BILL001", AmountPaid = 500m, PatientTestId = 999 };

        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Payment>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Payment p, CancellationToken ct) => { p.Id = 1; return p; });
        _mockPatientTestRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((PatientTest?)null);

        // Act
        var result = await _service.CreateAsync(payment);

        // Assert
        Assert.NotNull(result);
        _mockPatientTestRepository.Verify(r => r.UpdateAsync(It.IsAny<PatientTest>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenRepositoryThrows()
    {
        // Arrange
        var payment = new Payment { BillNo = "BILL001", AmountPaid = 500m };
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Payment>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.CreateAsync(payment));
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdatePayment()
    {
        // Arrange
        var payment = new Payment { Id = 1, BillNo = "BILL001", AmountPaid = 600m };
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Payment>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.UpdateAsync(payment);

        // Assert
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Payment>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldSetModifiedDateToUtcNow()
    {
        // Arrange
        var payment = new Payment { Id = 1, BillNo = "BILL001", AmountPaid = 500m };
        var beforeUpdate = DateTime.UtcNow;
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Payment>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.UpdateAsync(payment);
        var afterUpdate = DateTime.UtcNow;

        // Assert
        Assert.NotNull(payment.ModifiedDate);
        Assert.True(payment.ModifiedDate >= beforeUpdate && payment.ModifiedDate <= afterUpdate);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowException_WhenRepositoryThrows()
    {
        // Arrange
        var payment = new Payment { Id = 1, BillNo = "BILL001" };
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Payment>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(payment));
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeletePayment_WhenValidIdProvided()
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
    public async Task GetAllAsync_ShouldSupportCancellationToken()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        _mockRepository.Setup(r => r.GetAllAsync(cts.Token))
            .ReturnsAsync(new List<Payment>());

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
            .ReturnsAsync(new Payment());

        // Act
        await _service.GetByIdAsync(1, cts.Token);

        // Assert
        _mockRepository.Verify(r => r.GetByIdAsync(1, cts.Token), Times.Once);
    }

    [Fact]
    public async Task GetByBillNoAsync_ShouldSupportCancellationToken()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        _mockRepository.Setup(r => r.GetByBillNoAsync("BILL001", cts.Token))
            .ReturnsAsync(new List<Payment>());

        // Act
        await _service.GetByBillNoAsync("BILL001", cts.Token);

        // Assert
        _mockRepository.Verify(r => r.GetByBillNoAsync("BILL001", cts.Token), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldSupportCancellationToken()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        var payment = new Payment { BillNo = "BILL001", AmountPaid = 500m, PatientTestId = 1 };

        _mockRepository.Setup(r => r.AddAsync(payment, cts.Token))
            .ReturnsAsync(payment);
        _mockPatientTestRepository.Setup(r => r.GetByIdAsync(1, cts.Token))
            .ReturnsAsync((PatientTest?)null);

        // Act
        await _service.CreateAsync(payment, cts.Token);

        // Assert
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Payment>(), cts.Token), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldSupportCancellationToken()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        var payment = new Payment { Id = 1, BillNo = "BILL001" };
        _mockRepository.Setup(r => r.UpdateAsync(payment, cts.Token))
            .Returns(Task.CompletedTask);

        // Act
        await _service.UpdateAsync(payment, cts.Token);

        // Assert
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Payment>(), cts.Token), Times.Once);
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
}
