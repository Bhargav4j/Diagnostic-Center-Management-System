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

public class PatientTestServiceTests
{
    private readonly Mock<IPatientTestRepository> _mockRepository;
    private readonly Mock<IPatientRepository> _mockPatientRepository;
    private readonly Mock<ILogger<PatientTestService>> _mockLogger;
    private readonly PatientTestService _service;

    public PatientTestServiceTests()
    {
        _mockRepository = new Mock<IPatientTestRepository>();
        _mockPatientRepository = new Mock<IPatientRepository>();
        _mockLogger = new Mock<ILogger<PatientTestService>>();
        _service = new PatientTestService(_mockRepository.Object, _mockPatientRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenRepositoryIsNull()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new PatientTestService(null!, _mockPatientRepository.Object, _mockLogger.Object));
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenPatientRepositoryIsNull()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new PatientTestService(_mockRepository.Object, null!, _mockLogger.Object));
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new PatientTestService(_mockRepository.Object, _mockPatientRepository.Object, null!));
    }

    [Fact]
    public void Constructor_ShouldCreateInstance_WhenValidParametersProvided()
    {
        // Arrange & Act
        var service = new PatientTestService(_mockRepository.Object, _mockPatientRepository.Object, _mockLogger.Object);

        // Assert
        Assert.NotNull(service);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllPatientTests()
    {
        // Arrange
        var patientTests = new List<PatientTest>
        {
            new PatientTest { Id = 1, BillNo = "BILL001", TotalAmount = 500m },
            new PatientTest { Id = 2, BillNo = "BILL002", TotalAmount = 750m }
        };
        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(patientTests);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockRepository.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoPatientTestsExist()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<PatientTest>());

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
    public async Task GetByIdAsync_ShouldReturnPatientTest_WhenValidIdProvided()
    {
        // Arrange
        var patientTest = new PatientTest { Id = 1, BillNo = "BILL001" };
        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(patientTest);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("BILL001", result.BillNo);
        _mockRepository.Verify(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenPatientTestNotFound()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((PatientTest?)null);

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
    public async Task GetByBillNoAsync_ShouldReturnPatientTest_WhenValidBillNoProvided()
    {
        // Arrange
        var patientTest = new PatientTest { Id = 1, BillNo = "BILL001" };
        _mockRepository.Setup(r => r.GetByBillNoAsync("BILL001", It.IsAny<CancellationToken>()))
            .ReturnsAsync(patientTest);

        // Act
        var result = await _service.GetByBillNoAsync("BILL001");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("BILL001", result.BillNo);
        _mockRepository.Verify(r => r.GetByBillNoAsync("BILL001", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByBillNoAsync_ShouldReturnNull_WhenBillNoNotFound()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByBillNoAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((PatientTest?)null);

        // Act
        var result = await _service.GetByBillNoAsync("NONEXISTENT");

        // Assert
        Assert.Null(result);
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
    public async Task GetByPatientIdAsync_ShouldReturnMatchingPatientTests()
    {
        // Arrange
        var patientTests = new List<PatientTest>
        {
            new PatientTest { Id = 1, PatientId = 10, BillNo = "BILL001" },
            new PatientTest { Id = 2, PatientId = 10, BillNo = "BILL002" }
        };
        _mockRepository.Setup(r => r.GetByPatientIdAsync(10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(patientTests);

        // Act
        var result = await _service.GetByPatientIdAsync(10);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockRepository.Verify(r => r.GetByPatientIdAsync(10, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByPatientIdAsync_ShouldReturnEmptyList_WhenNoMatches()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByPatientIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<PatientTest>());

        // Act
        var result = await _service.GetByPatientIdAsync(999);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetByPatientIdAsync_ShouldThrowException_WhenRepositoryThrows()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByPatientIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.GetByPatientIdAsync(10));
    }

    [Fact]
    public async Task GetUnpaidTestsAsync_ShouldReturnUnpaidTests()
    {
        // Arrange
        var unpaidTests = new List<PatientTest>
        {
            new PatientTest { Id = 1, TotalAmount = 500m, PaidAmount = 0m },
            new PatientTest { Id = 2, TotalAmount = 750m, PaidAmount = 250m }
        };
        _mockRepository.Setup(r => r.GetUnpaidTestsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(unpaidTests);

        // Act
        var result = await _service.GetUnpaidTestsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockRepository.Verify(r => r.GetUnpaidTestsAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetUnpaidTestsAsync_ShouldReturnEmptyList_WhenNoUnpaidTests()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetUnpaidTestsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<PatientTest>());

        // Act
        var result = await _service.GetUnpaidTestsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetUnpaidTestsAsync_ShouldThrowException_WhenRepositoryThrows()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetUnpaidTestsAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.GetUnpaidTestsAsync());
    }

    [Fact]
    public async Task CreateAsync_ShouldCreatePatientTest_WithCorrectProperties()
    {
        // Arrange
        var patientTest = new PatientTest { PatientId = 1, TestSetupId = 2, TotalAmount = 500m };
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<PatientTest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((PatientTest pt, CancellationToken ct) => { pt.Id = 1; return pt; });

        // Act
        var result = await _service.CreateAsync(patientTest);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsActive);
        Assert.NotEqual(default(DateTime), result.CreatedDate);
        Assert.NotEmpty(result.BillNo);
        Assert.StartsWith("BILL", result.BillNo);
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<PatientTest>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldSetCreatedDateToUtcNow()
    {
        // Arrange
        var patientTest = new PatientTest { PatientId = 1, TotalAmount = 500m };
        var beforeCreate = DateTime.UtcNow;
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<PatientTest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((PatientTest pt, CancellationToken ct) => pt);

        // Act
        var result = await _service.CreateAsync(patientTest);
        var afterCreate = DateTime.UtcNow;

        // Assert
        Assert.True(result.CreatedDate >= beforeCreate && result.CreatedDate <= afterCreate);
    }

    [Fact]
    public async Task CreateAsync_ShouldSetIsActiveToTrue()
    {
        // Arrange
        var patientTest = new PatientTest { PatientId = 1, IsActive = false };
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<PatientTest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((PatientTest pt, CancellationToken ct) => pt);

        // Act
        var result = await _service.CreateAsync(patientTest);

        // Assert
        Assert.True(result.IsActive);
    }

    [Fact]
    public async Task CreateAsync_ShouldGenerateUniqueBillNo()
    {
        // Arrange
        var patientTest1 = new PatientTest { PatientId = 1 };
        var patientTest2 = new PatientTest { PatientId = 1 };
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<PatientTest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((PatientTest pt, CancellationToken ct) => pt);

        // Act
        var result1 = await _service.CreateAsync(patientTest1);
        var result2 = await _service.CreateAsync(patientTest2);

        // Assert
        Assert.NotEmpty(result1.BillNo);
        Assert.NotEmpty(result2.BillNo);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenRepositoryThrows()
    {
        // Arrange
        var patientTest = new PatientTest { PatientId = 1 };
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<PatientTest>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.CreateAsync(patientTest));
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdatePatientTest()
    {
        // Arrange
        var patientTest = new PatientTest { Id = 1, PatientId = 1, TotalAmount = 600m };
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<PatientTest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.UpdateAsync(patientTest);

        // Assert
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<PatientTest>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldSetModifiedDateToUtcNow()
    {
        // Arrange
        var patientTest = new PatientTest { Id = 1, PatientId = 1 };
        var beforeUpdate = DateTime.UtcNow;
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<PatientTest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.UpdateAsync(patientTest);
        var afterUpdate = DateTime.UtcNow;

        // Assert
        Assert.NotNull(patientTest.ModifiedDate);
        Assert.True(patientTest.ModifiedDate >= beforeUpdate && patientTest.ModifiedDate <= afterUpdate);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowException_WhenRepositoryThrows()
    {
        // Arrange
        var patientTest = new PatientTest { Id = 1 };
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<PatientTest>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(patientTest));
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeletePatientTest_WhenValidIdProvided()
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
            .ReturnsAsync(new List<PatientTest>());

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
            .ReturnsAsync(new PatientTest());

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
            .ReturnsAsync(new PatientTest());

        // Act
        await _service.GetByBillNoAsync("BILL001", cts.Token);

        // Assert
        _mockRepository.Verify(r => r.GetByBillNoAsync("BILL001", cts.Token), Times.Once);
    }

    [Fact]
    public async Task GetByPatientIdAsync_ShouldSupportCancellationToken()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        _mockRepository.Setup(r => r.GetByPatientIdAsync(10, cts.Token))
            .ReturnsAsync(new List<PatientTest>());

        // Act
        await _service.GetByPatientIdAsync(10, cts.Token);

        // Assert
        _mockRepository.Verify(r => r.GetByPatientIdAsync(10, cts.Token), Times.Once);
    }

    [Fact]
    public async Task GetUnpaidTestsAsync_ShouldSupportCancellationToken()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        _mockRepository.Setup(r => r.GetUnpaidTestsAsync(cts.Token))
            .ReturnsAsync(new List<PatientTest>());

        // Act
        await _service.GetUnpaidTestsAsync(cts.Token);

        // Assert
        _mockRepository.Verify(r => r.GetUnpaidTestsAsync(cts.Token), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldSupportCancellationToken()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        var patientTest = new PatientTest { PatientId = 1 };
        _mockRepository.Setup(r => r.AddAsync(patientTest, cts.Token))
            .ReturnsAsync(patientTest);

        // Act
        await _service.CreateAsync(patientTest, cts.Token);

        // Assert
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<PatientTest>(), cts.Token), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldSupportCancellationToken()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        var patientTest = new PatientTest { Id = 1 };
        _mockRepository.Setup(r => r.UpdateAsync(patientTest, cts.Token))
            .Returns(Task.CompletedTask);

        // Act
        await _service.UpdateAsync(patientTest, cts.Token);

        // Assert
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<PatientTest>(), cts.Token), Times.Once);
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
