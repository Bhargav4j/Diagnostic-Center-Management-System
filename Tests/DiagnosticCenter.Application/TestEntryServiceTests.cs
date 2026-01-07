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

public class TestEntryServiceTests
{
    private readonly Mock<ITestEntryRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<TestEntryService>> _mockLogger;
    private readonly TestEntryService _service;

    public TestEntryServiceTests()
    {
        _mockRepository = new Mock<ITestEntryRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<TestEntryService>>();
        _service = new TestEntryService(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
    }

    [Fact]
    public void Constructor_WithNullRepository_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new TestEntryService(null, _mockMapper.Object, _mockLogger.Object));
    }

    [Fact]
    public void Constructor_WithNullMapper_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new TestEntryService(_mockRepository.Object, null, _mockLogger.Object));
    }

    [Fact]
    public void Constructor_WithNullLogger_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new TestEntryService(_mockRepository.Object, _mockMapper.Object, null));
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllTestEntries()
    {
        var testEntries = new List<TestEntry> { new TestEntry { Id = 1, PatientName = "John Doe" } };
        var testEntryDtos = new List<TestEntryDto> { new TestEntryDto { Id = 1, PatientName = "John Doe" } };
        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(testEntries);
        _mockMapper.Setup(m => m.Map<IEnumerable<TestEntryDto>>(testEntries)).Returns(testEntryDtos);

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
    public async Task GetByIdAsync_WithValidId_ReturnsTestEntry()
    {
        var testEntry = new TestEntry { Id = 1, PatientName = "John Doe" };
        var testEntryDto = new TestEntryDto { Id = 1, PatientName = "John Doe" };
        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(testEntry);
        _mockMapper.Setup(m => m.Map<TestEntryDto>(testEntry)).Returns(testEntryDto);

        var result = await _service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ReturnsNull()
    {
        _mockRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync((TestEntry)null);

        var result = await _service.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByBillNoAsync_WithValidBillNo_ReturnsTestEntry()
    {
        var billNo = "BILL001";
        var testEntry = new TestEntry { Id = 1, BillNo = billNo, PatientName = "John Doe" };
        var testEntryDto = new TestEntryDto { Id = 1, BillNo = billNo, PatientName = "John Doe" };
        _mockRepository.Setup(r => r.GetByBillNoAsync(billNo, It.IsAny<CancellationToken>())).ReturnsAsync(testEntry);
        _mockMapper.Setup(m => m.Map<TestEntryDto>(testEntry)).Returns(testEntryDto);

        var result = await _service.GetByBillNoAsync(billNo);

        Assert.NotNull(result);
        Assert.Equal(billNo, result.BillNo);
    }

    [Fact]
    public async Task GetByBillNoAsync_WithInvalidBillNo_ReturnsNull()
    {
        var billNo = "INVALID";
        _mockRepository.Setup(r => r.GetByBillNoAsync(billNo, It.IsAny<CancellationToken>())).ReturnsAsync((TestEntry)null);

        var result = await _service.GetByBillNoAsync(billNo);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_WithValidDto_ReturnsCreatedTestEntry()
    {
        var createDto = new TestEntryCreateDto { PatientName = "John Doe" };
        var testEntry = new TestEntry { Id = 1, PatientName = "John Doe" };
        var testEntryDto = new TestEntryDto { Id = 1, PatientName = "John Doe" };
        _mockMapper.Setup(m => m.Map<TestEntry>(createDto)).Returns(testEntry);
        _mockRepository.Setup(r => r.AddAsync(testEntry, It.IsAny<CancellationToken>())).ReturnsAsync(testEntry);
        _mockMapper.Setup(m => m.Map<TestEntryDto>(testEntry)).Returns(testEntryDto);

        var result = await _service.CreateAsync(createDto);

        Assert.NotNull(result);
        Assert.Equal("John Doe", result.PatientName);
    }

    [Fact]
    public async Task CreateAsync_WithException_RethrowsException()
    {
        var createDto = new TestEntryCreateDto { PatientName = "John Doe" };
        _mockMapper.Setup(m => m.Map<TestEntry>(createDto)).Throws(new Exception("Test exception"));

        await Assert.ThrowsAsync<Exception>(() => _service.CreateAsync(createDto));
    }

    [Fact]
    public async Task UpdateAsync_WithValidId_UpdatesTestEntry()
    {
        var updateDto = new TestEntryUpdateDto { PatientName = "Jane Doe" };
        var existing = new TestEntry { Id = 1, PatientName = "John Doe" };
        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(existing);
        _mockMapper.Setup(m => m.Map(updateDto, existing));
        _mockRepository.Setup(r => r.UpdateAsync(existing, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        await _service.UpdateAsync(1, updateDto);

        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<TestEntry>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithInvalidId_ThrowsInvalidOperationException()
    {
        var updateDto = new TestEntryUpdateDto { PatientName = "Jane Doe" };
        _mockRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync((TestEntry)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.UpdateAsync(999, updateDto));
    }

    [Fact]
    public async Task DeleteAsync_WithValidId_DeletesTestEntry()
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
    public async Task SearchAsync_WithValidTerm_ReturnsMatchingTestEntries()
    {
        var searchTerm = "John";
        var testEntries = new List<TestEntry> { new TestEntry { Id = 1, PatientName = "John Doe" } };
        var testEntryDtos = new List<TestEntryDto> { new TestEntryDto { Id = 1, PatientName = "John Doe" } };
        _mockRepository.Setup(r => r.SearchAsync(searchTerm, It.IsAny<CancellationToken>())).ReturnsAsync(testEntries);
        _mockMapper.Setup(m => m.Map<IEnumerable<TestEntryDto>>(testEntries)).Returns(testEntryDtos);

        var result = await _service.SearchAsync(searchTerm);

        Assert.NotNull(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task SearchAsync_WithEmptyTerm_ReturnsEmptyList()
    {
        var searchTerm = "";
        var testEntries = new List<TestEntry>();
        var testEntryDtos = new List<TestEntryDto>();
        _mockRepository.Setup(r => r.SearchAsync(searchTerm, It.IsAny<CancellationToken>())).ReturnsAsync(testEntries);
        _mockMapper.Setup(m => m.Map<IEnumerable<TestEntryDto>>(testEntries)).Returns(testEntryDtos);

        var result = await _service.SearchAsync(searchTerm);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetUnpaidEntriesAsync_ReturnsUnpaidEntries()
    {
        var testEntries = new List<TestEntry> { new TestEntry { Id = 1, PatientName = "John Doe", IsPaid = false } };
        var testEntryDtos = new List<TestEntryDto> { new TestEntryDto { Id = 1, PatientName = "John Doe" } };
        _mockRepository.Setup(r => r.GetUnpaidEntriesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(testEntries);
        _mockMapper.Setup(m => m.Map<IEnumerable<TestEntryDto>>(testEntries)).Returns(testEntryDtos);

        var result = await _service.GetUnpaidEntriesAsync();

        Assert.NotNull(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetUnpaidEntriesAsync_WithNoUnpaidEntries_ReturnsEmptyList()
    {
        var testEntries = new List<TestEntry>();
        var testEntryDtos = new List<TestEntryDto>();
        _mockRepository.Setup(r => r.GetUnpaidEntriesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(testEntries);
        _mockMapper.Setup(m => m.Map<IEnumerable<TestEntryDto>>(testEntries)).Returns(testEntryDtos);

        var result = await _service.GetUnpaidEntriesAsync();

        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
