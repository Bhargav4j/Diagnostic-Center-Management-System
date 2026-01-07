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

public class PaymentServiceTests
{
    private readonly Mock<IPaymentRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<PaymentService>> _mockLogger;
    private readonly PaymentService _service;

    public PaymentServiceTests()
    {
        _mockRepository = new Mock<IPaymentRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<PaymentService>>();
        _service = new PaymentService(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
    }

    [Fact]
    public void Constructor_WithNullRepository_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new PaymentService(null, _mockMapper.Object, _mockLogger.Object));
    }

    [Fact]
    public void Constructor_WithNullMapper_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new PaymentService(_mockRepository.Object, null, _mockLogger.Object));
    }

    [Fact]
    public void Constructor_WithNullLogger_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new PaymentService(_mockRepository.Object, _mockMapper.Object, null));
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllPayments()
    {
        var payments = new List<Payment> { new Payment { Id = 1, Amount = 100 } };
        var paymentDtos = new List<PaymentDto> { new PaymentDto { Id = 1, Amount = 100 } };
        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(payments);
        _mockMapper.Setup(m => m.Map<IEnumerable<PaymentDto>>(payments)).Returns(paymentDtos);

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
    public async Task GetByIdAsync_WithValidId_ReturnsPayment()
    {
        var payment = new Payment { Id = 1, Amount = 100 };
        var paymentDto = new PaymentDto { Id = 1, Amount = 100 };
        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(payment);
        _mockMapper.Setup(m => m.Map<PaymentDto>(payment)).Returns(paymentDto);

        var result = await _service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ReturnsNull()
    {
        _mockRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync((Payment)null);

        var result = await _service.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_WithValidDto_ReturnsCreatedPayment()
    {
        var createDto = new PaymentCreateDto { TestEntryId = 1, Amount = 100 };
        var payment = new Payment { Id = 1, TestEntryId = 1, Amount = 100 };
        var paymentDto = new PaymentDto { Id = 1, TestEntryId = 1, Amount = 100 };
        _mockMapper.Setup(m => m.Map<Payment>(createDto)).Returns(payment);
        _mockRepository.Setup(r => r.AddAsync(payment, It.IsAny<CancellationToken>())).ReturnsAsync(payment);
        _mockMapper.Setup(m => m.Map<PaymentDto>(payment)).Returns(paymentDto);

        var result = await _service.CreateAsync(createDto);

        Assert.NotNull(result);
        Assert.Equal(100, result.Amount);
    }

    [Fact]
    public async Task CreateAsync_WithException_RethrowsException()
    {
        var createDto = new PaymentCreateDto { TestEntryId = 1, Amount = 100 };
        _mockMapper.Setup(m => m.Map<Payment>(createDto)).Throws(new Exception("Test exception"));

        await Assert.ThrowsAsync<Exception>(() => _service.CreateAsync(createDto));
    }

    [Fact]
    public async Task UpdateAsync_WithValidId_UpdatesPayment()
    {
        var updateDto = new PaymentUpdateDto { Amount = 200 };
        var existing = new Payment { Id = 1, Amount = 100 };
        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(existing);
        _mockMapper.Setup(m => m.Map(updateDto, existing));
        _mockRepository.Setup(r => r.UpdateAsync(existing, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        await _service.UpdateAsync(1, updateDto);

        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Payment>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithInvalidId_ThrowsInvalidOperationException()
    {
        var updateDto = new PaymentUpdateDto { Amount = 200 };
        _mockRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync((Payment)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.UpdateAsync(999, updateDto));
    }

    [Fact]
    public async Task DeleteAsync_WithValidId_DeletesPayment()
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
    public async Task GetByTestEntryIdAsync_WithValidId_ReturnsPayments()
    {
        var testEntryId = 1;
        var payments = new List<Payment> { new Payment { Id = 1, TestEntryId = 1, Amount = 100 } };
        var paymentDtos = new List<PaymentDto> { new PaymentDto { Id = 1, TestEntryId = 1, Amount = 100 } };
        _mockRepository.Setup(r => r.GetByTestEntryIdAsync(testEntryId, It.IsAny<CancellationToken>())).ReturnsAsync(payments);
        _mockMapper.Setup(m => m.Map<IEnumerable<PaymentDto>>(payments)).Returns(paymentDtos);

        var result = await _service.GetByTestEntryIdAsync(testEntryId);

        Assert.NotNull(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetByTestEntryIdAsync_WithNoPayments_ReturnsEmptyList()
    {
        var testEntryId = 999;
        var payments = new List<Payment>();
        var paymentDtos = new List<PaymentDto>();
        _mockRepository.Setup(r => r.GetByTestEntryIdAsync(testEntryId, It.IsAny<CancellationToken>())).ReturnsAsync(payments);
        _mockMapper.Setup(m => m.Map<IEnumerable<PaymentDto>>(payments)).Returns(paymentDtos);

        var result = await _service.GetByTestEntryIdAsync(testEntryId);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task CreateAsync_WithNegativeAmount_CreatesPayment()
    {
        var createDto = new PaymentCreateDto { TestEntryId = 1, Amount = -50 };
        var payment = new Payment { Id = 1, TestEntryId = 1, Amount = -50 };
        var paymentDto = new PaymentDto { Id = 1, TestEntryId = 1, Amount = -50 };
        _mockMapper.Setup(m => m.Map<Payment>(createDto)).Returns(payment);
        _mockRepository.Setup(r => r.AddAsync(payment, It.IsAny<CancellationToken>())).ReturnsAsync(payment);
        _mockMapper.Setup(m => m.Map<PaymentDto>(payment)).Returns(paymentDto);

        var result = await _service.CreateAsync(createDto);

        Assert.NotNull(result);
        Assert.Equal(-50, result.Amount);
    }
}
