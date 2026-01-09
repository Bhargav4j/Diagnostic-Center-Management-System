using Xunit;
using Moq;
using DiagnosticCenter.Application.Services;
using DiagnosticCenter.Domain.Entities;
using DiagnosticCenter.Domain.Exceptions;
using DiagnosticCenter.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DiagnosticCenter.UnitTests.Services;

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
        var testSetups = new List<TestSetup>
        {
            new TestSetup { Id = 1, Name = "Test 1" },
            new TestSetup { Id = 2, Name = "Test 2" }
        };
        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(testSetups);
        var result = await _service.GetAllAsync();
        Assert.NotNull(result);
        _mockRepository.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnTestSetup()
    {
        var testSetup = new TestSetup { Id = 1, Name = "Test" };
        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(testSetup);
        var result = await _service.GetByIdAsync(1);
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }
}
